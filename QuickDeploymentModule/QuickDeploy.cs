using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using QuickDeploymentModule.Properties;
using Sage.Platform.Application;
using Sage.Platform.Deployment;
using Sage.Platform.Deployment.JSBuilder;
using Sage.Platform.FileSystem.Interfaces;
using Sage.Platform.Projects;
using Sage.Platform.Projects.Interfaces;
using Sage.Platform.WebPortal.Design;
using DeploymentTarget = Sage.Platform.Deployment.DeploymentTarget;
using Sage.Platform.FileSystem;

namespace QuickDeploymentModule
{
    /// <summary>
    /// Adapted from the Sage.Platform.Deployment.PortalDeploymentManager
    /// </summary>
    /// <remarks>Features missing:
    /// Cancelling
    /// Virtual directory configuration
    /// deployment manifests
    /// cleanup (removing files no longer used)
    /// No ServiceHosts.xml file generated - for process orchestration?
    /// No progress Dialog - messages are in output window
    /// </remarks>
    public class QuickDeploy
    {
        private DeploymentTarget _target;
        private DeploymentTargetPortal _targetPortal;
        private PortalApplication _portal;
        private PortalDeploymentManager _manager;
        private BackgroundWorker _worker;
        private ILog _outputLog = LogManager.GetLogger("Sage.Build");

        public QuickDeploy(DeploymentTarget target, DeploymentTargetPortal targetPortal)
        {            
            _target = target;
            _targetPortal = targetPortal;

            var projectContext = ApplicationContext.Current.Services.Get<IProjectContextService>(true);
            var portalModel = projectContext.ActiveProject.Models.Get<PortalModel>();
            Guid portalId = new Guid(_targetPortal.InstanceId);
            _portal = portalModel.PortalApplications.FirstOrDefault(portal => portal.Id == portalId);
            if (_portal == null)
                throw new Exception(string.Format("Could not find portal application by Id: {0}", portalId));

            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.DoWork += (sender, e) => InternalDeploy();
        }

        public void Deploy()
        {
            _worker.RunWorkerAsync();    
        }

        private void InternalDeploy()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();
                _manager = new PortalDeploymentManager();
                List<IDeploymentActionProvider> deploymentActions = _manager.GetDeploymentActions(_portal);

                //Before Deployment Actions
                deploymentActions.ForEach(provider => provider.BeforeDeployment(_portal, _worker));

                //retrieve and deploy files
                ShadowCopyItemCollection deploymentItems = GetPortalDeploymentItems(deploymentActions);
                var copyStart = watch.Elapsed.TotalSeconds;
                DeployTarget(deploymentItems);
                var copyEnd = watch.Elapsed.TotalSeconds;
                ReportProgress(string.Format("Copying files to target took {0} seconds.", copyEnd - copyStart));
                
                //Delete any target manifest
                var targetManifest = FileSystem.GetFileInfo(System.IO.Path.Combine(
                    _target.GetFolderName(_targetPortal), "deployables.manifest.xml"));
                if (targetManifest.Exists)
                    targetManifest.Delete();

                //AfterDeployment actions
                deploymentActions.ForEach(provider => provider.AfterDeployment(_portal, _worker));

                //cleanup any open streams
                deploymentItems.ForEach(item =>
                                            {
                                                if (!(item is LazyShadowCopyItem) && item.Data != null)
                                                    item.Data.Dispose();
                                            });

                watch.Stop();
                ReportProgress(string.Format("Deployment took {0} seconds.", watch.Elapsed.TotalSeconds));
            }
            catch (Exception ex)
            {
                _outputLog.ErrorFormat("Error during quick deploy...");
                _outputLog.ErrorFormat(ex.Message);
                if (ex.GetBaseException() != ex)
                    _outputLog.ErrorFormat(ex.GetBaseException().Message);
                _outputLog.ErrorFormat(ex.StackTrace);
            }
        }

        private ShadowCopyItemCollection GetPortalDeploymentItems(List<IDeploymentActionProvider> deploymentActions)
        {
            ReportProgress(Resources.Status_Deploy_Get_Items);

            ShadowCopyItemCollection deploymentItems = GetAllDeployables();
            ReportProgress(Resources.CustomDeploymentMessage);

            // Get any items based on the deployment actions
            deploymentActions.ForEach(provider =>
            {
                try
                {
                    IShadowCopyItem[] providerItems = provider.GetDeployableItems(_portal, _worker);
                    if (providerItems != null)
                    {
                        Array.ForEach(providerItems, item =>
                        {
                            if (deploymentItems != null &&
                                !deploymentItems.ContainsUrl(item.Url))
                                deploymentItems.Add(item);
                        });
                    }
                }
                catch (Exception ex)
                {
                    _outputLog.ErrorFormat(Resources.Error_No_Provider_Items, provider.ToString());
                    _outputLog.ErrorFormat(ex.Message);
                    _outputLog.ErrorFormat(ex.StackTrace);
                }
            });

            return deploymentItems;
        }

        private bool DeployTarget(ShadowCopyItemCollection deploymentItems)
        {
            bool result = false;

            string path = _target.GetFolderName(_targetPortal);

            if (!string.IsNullOrEmpty(path))
            {
                // Copy the files to the target
                ReportProgress(Resources.Status_Copy_Target_Files);
                CopyPortalItemsToTarget(deploymentItems);

                try
                {
                    JSBuilderAction action = new JSBuilderAction();
                    action.Execute(path);
                }
                catch (Exception) { }

                result = true;
            }
            //else
            //    OutputError(Resources.Info_Target_Folder_Is_Invalid);

            return result;
        }

        private void ReportProgress(string message)
        {
            _outputLog.Info(message);
        }

        private ShadowCopyItemCollection GetAllDeployables()
        {
            const string CONST_APPCONFIGFILE = "application.xml";

            ShadowCopyItemCollection results = new ShadowCopyItemCollection();

            ReportProgress(Resources.GatheringDeploymentFiles);

            //Application Configuration
            CallWithTiming("Generating application.xml", () => {
                Stream appConfigStream = (Stream) CallPrivateMethod("ToApplicationConfigFile", new object[] { _portal });
                results.Add(new ShadowCopyItem(CONST_APPCONFIGFILE, appConfigStream));
                });

            CallWithTiming("Getting support files", () => AddSupportFilesToDeployables(results));
            
            CallWithTiming("Getting navitems", 
                () => CallPrivateMethod("AddNavItemsToDeployables", new object[] { _portal, false, results, _worker }));

            CallWithTiming("Getting menu items", 
                () => CallPrivateMethod("AddMenuItemsToDeployables", new object[] { _portal, false, results, _worker }));

            CallWithTiming("Getting pages", () => AddPagesToDeployables(results));

            CallWithTiming("Getting smart part mappings",
                () => CallPrivateMethod("AddSmartPartMappingsToDeployables", new object[] { _portal, results, _worker }));

            CallWithTiming("Getting resources",
                () => CallPrivateMethod("AddResourcesToDeployables", new object[] { _portal, results, _worker }));

            return results;
        }

        private void CallWithTiming(string messagePrefix, Action action)
        {
            ReportProgress(messagePrefix + "...");
            var watch = new Stopwatch();
            watch.Start();
            action();
            watch.Stop();
            ReportProgress(string.Format("{0} took {1} seconds.", messagePrefix, watch.Elapsed.TotalSeconds));
        }

        private object CallPrivateMethod(string methodName, object[] parameters)
        {
            MethodInfo method = _manager.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (method == null)
                throw new Exception(string.Format("Could not find PortalDeploymentManager.{0}() method.", methodName));

            object result = method.Invoke(_manager, parameters);

            return result;           
        }

        public void CopyPortalItemsToTarget(ShadowCopyItemCollection deploymentItems)
        {
            IDeploymentTarget target = DeploymentTargetConfigurationManager.GetMappedTarget(_target.TargetType);

            string deploymentPath = target.GetDeploymentPath(_target, _targetPortal);
            if (string.IsNullOrEmpty(deploymentPath)) 
                return;
            
            for (int i = 0; i < deploymentItems.Count; i++)
            {
                IShadowCopyItem item = deploymentItems[i];
                LazyShadowCopyItem lazyItem = item as LazyShadowCopyItem;

                FileInfo targetFile = new FileInfo(System.IO.Path.Combine(deploymentPath, item.Url));

                if (ItemNeedsToBeDeployed(item, targetFile))
                {
                    string targetPath = System.IO.Path.Combine(deploymentPath.TrimEnd('\\'), item.Url);
                    IFileInfo file = FileSystem.GetFileInfo(targetPath);

                    if (!file.Directory.Exists)
                        file.Directory.Create();

                    if (lazyItem != null)
                    {
                        FSFile.Copy(FileSystem.GetFileInfo(lazyItem.FullPath), file, true);
                    }
                    else
                    {
                        using (Stream writeStream = file.Open(FileMode.Create, FileAccess.Write))
                        {
                            FSFile.CopyStream(item.Data, writeStream);
                        }
                    }
                }
            }
        }

        public bool ItemNeedsToBeDeployed(IShadowCopyItem sourceItem, FileInfo targetFile)
        {
            if (!targetFile.Exists)
                return true;

            LazyShadowCopyItem lazyItem = sourceItem as LazyShadowCopyItem;

            if (lazyItem != null)
            {
                return (lazyItem.Size != targetFile.Length) || (lazyItem.ModifiedDate != targetFile.LastWriteTimeUtc);
            }
                
            return true;
        }

        private void AddSupportFilesToDeployables(ShadowCopyItemCollection deployables)
        {
            LinkedFile[] supportFiles = _portal.SupportFiles.GetFiles(true);

            foreach (LinkedFile item in supportFiles)
            {
                string relativeUrl = _manager.SupportFilePathToAppRelativePath(item.ProjectPath);

                if (PortalUtil.IsShadowCopyFile(item.ProjectPath) && item.Source.Exists)
                    deployables.Add(new LazyShadowCopyItem(item.Source.FullName, relativeUrl));
            }
        }

        private void AddPagesToDeployables(ShadowCopyItemCollection deployables)
        {
            var pageGen = new QuickPageGenerator(_portal.Project);

            foreach (PortalPage page in _portal.AllPages)
            {
                page.Validate(true);

                IShadowCopyProvider provider = new QuickPageGenerationProvider(pageGen, page, _portal);

                deployables.AddRange(provider.GetItems(_worker));
            }
        }

    }
}