//-----------------------------------------------------------------------
// <copyright file="BuildManager.cs" company="Sage Software">
//     Copyright (c) Sage Software. All rights reserved.
//		This code may not be copied or used, except as set out in a written licence agreement
// 		between the user and Sage Software, which specifically permits the user to use
// 		this code. 
// </copyright>
//-----------------------------------------------------------------------



using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Web.Compilation;
using System.Xml.Serialization;
using log4net;
using log4net.Config;
using Sage.Platform.Configuration;
using ConfigurationManager = Sage.Platform.Configuration.ConfigurationManager;
using Sage.Platform.Application;
using Sage.Platform.VirtualFileSystem;
using Sage.SalesLogix.Web;
using Sage.Platform.Security;
using Sage.Platform.Data;
using Sage.Platform.Extensibility;
using Sage.Platform.Projects;
using Sage.Platform.Projects.Interfaces;
using Sage.Platform.Extensibility.Services;
using Sage.Platform.Threading;
using Sage.Platform.Extensibility.Interfaces;
using Sage.Platform.Projects.Localization;
using Sage.Platform.Orm.Entities;
using Sage.Platform.DynamicMethod;
using Sage.Platform.FileSystem.Interfaces;
using Sage.Platform.Orm;
using Sage.Platform.FileSystem;
using Sage.Platform.IDEModule;
using Sage.Platform.WebPortal.Design;
using Sage.Platform;
using Sage.Platform.Deployment;
using SalesLogix.Deployment.Properties;

namespace SalesLogix.Deployment
{
    /// <summary>
    /// Provides support to execute the build and deployment
    /// </summary>
    public class BuildManager
    {
        #region Fields
        private ILog log = LogManager.GetLogger("DeploymentUtility.Program");
        #endregion

        #region Public properties
        public BuildManifest Manifest { get; private set; }
        Sage.Platform.Configuration.ConfigurationManager ConfigManager { get; set; }
        #endregion

        #region CTOR
        public BuildManager(BuildManifest manifest)
        {
            Manifest = manifest;
        }
        #endregion

        public void Build()
        {

            ApplicationContext.Initialize("DeploymentUtility");
            XmlConfigurator.Configure();


            if (!string.IsNullOrEmpty(Manifest.VFSPath))
            {
                if (!Directory.Exists(Manifest.VFSPath))
                {
                    log.Info("VFS path provided does not exist.");
                    return;
                }

                if (!Directory.Exists(Path.Combine(Manifest.VFSPath, "Entity Model")))
                {
                    log.Info("It looks as if the VFS path is not pointing to the Model Directory. Please recheck your VFS Path");
                    return;
                }
            }

            string connString = Manifest.ConnectionString;

            log.Info("DeploymentUtility");

            ServiceCollection services = InitializeServices(connString);
            SetRunningIdentity();

            if (Utility.CanConnect(connString))
            {

                VFSQuery.ConnectToVFS(connString);
                InitializeVFS(connString);

                ConfigManager = services.Get<ConfigurationManager>();
                ConfigManager.RegisterConfigurationType(typeof(WebUserConfiguration));

                BuildApplicationCore(Manifest.BuildCore);

                string deploymentName = Manifest.DeploymentName;

                if (!string.IsNullOrEmpty(deploymentName))
                {

                    // Support for multiple deployments 
                    if (deploymentName.IndexOf(",") > -1)
                    {
                        string[] deployments = deploymentName.Split(',');

                        foreach (string deployment in deployments)
                        {
                            log.InfoFormat(Resources.msg_processing_deployment, deployment);
                            DeployTarget(deployment.Trim());
                        }

                    }
                    else
                    {
                        log.InfoFormat(Resources.msg_processing_deployment, deploymentName);
                        DeployTarget(deploymentName);
                    }

                }
                else
                {
                    log.InfoFormat(Resources.msg_deploying_portal, Manifest.PortalName);
                    DeployPortal();
                }

            }
        }

        private static ServiceCollection InitializeServices(string connectionString)
        {
            ServiceCollection services = ApplicationContext.Current.RootWorkItem.Services;
            services.Add(typeof(IUserService), new UserService());
         
            ConnectionStringDataService connectionStringDataService = new ConnectionStringDataService(connectionString);
            services.Add(typeof(IDataService), connectionStringDataService);
            
            return services;
        }

        private void SetRunningIdentity()
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("ADMIN", "user"), null);
        }



        /// <summary>
        /// Handles the process of building the core components
        /// </summary>
        private void BuildApplicationCore(bool fullBuild)
        {
            ServiceCollection services = ApplicationContext.Current.Services;

            var cm = services.Get<Sage.Platform.Configuration.ConfigurationManager>(true);
            cm.RegisterConfigurationType(typeof(BuildSettings));

            var settings = cm.GetConfiguration<BuildSettings>();
            settings.SolutionFolder = Manifest.OutputPath;
            settings.AcceptChanges();

            cm.WriteConfiguration(settings);

            var modelTypes = new ModelTypeCollection();
            RegisterRequiredModels(modelTypes);

            string vfsPath = string.Empty;
            if (string.IsNullOrEmpty(Manifest.VFSPath))
                vfsPath = ProjectWorkspace.VFS_MODEL_PATH;
            else
                vfsPath = Manifest.VFSPath;

            var workspace = new ProjectWorkspace(vfsPath)
            {
                Name = "TEMP_VFS"
            };

            using (IProject project = new Project(workspace, modelTypes))
            {

                RegisterNeededServices(services, project);

                var service = (Platforms)ExtensionManager.Default.GetService(typeof(Platforms));

                if (fullBuild)
                {
                    log.Info(Resources.log_building_core);

                    try
                    {

                        VFSQuery.UpgradeToBatchMode();
                        var files = new List<string>();

                        // Build Interfaces
                        string[] interfaceFiles = BuildPlatform(project, service, PlatformGuids.CommonGuid);
                        files.AddRange(interfaceFiles);
                        OnBuildComplete(files.ToArray());
                        files.AddRange(BuildPlatform(project, service, PlatformGuids.WebGuid));


                        GenerateCodeSnippetLibraries(project);
                        GenerateRulesConfiguration(project);

                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        VFSQuery.DowngradeFromBatchMode();
                    }

                }
            }
        }


        /// <summary>
        /// Register the required models (VFS)
        /// </summary>
        /// <param name="modelTypes"></param>
        private static void RegisterRequiredModels(ModelTypeCollection modelTypes)
        {
            RegisterModelType(modelTypes, "Sage.Platform.Orm.Entities.OrmModel, Sage.Platform");
            RegisterModelType(modelTypes, "Sage.Platform.QuickForms.QuickFormModel, Sage.Platform.QuickForms");
            RegisterModelType(modelTypes, "Sage.Platform.WebPortal.Design.PortalModel, Sage.Platform.WebPortal.Design");
            RegisterModelType(modelTypes, "Sage.Platform.BundleModel.BundleModel, Sage.Platform.BundleModel");
            RegisterModelType(modelTypes, "Sage.Platform.Mashups.AdminModule.MashupModel, Sage.Platform.Mashups.AdminModule");
        }

        private static void RegisterModelType(ModelTypeCollection modelTypes, string qualifiedName)
        {
            var mt = new ModelType { ModelTypeAssemblyQualifiedName = qualifiedName };
            modelTypes.Add(mt);
        }

        /// <summary>
        /// Handles building the platform components 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="service"></param>
        /// <param name="components"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        private string[] BuildPlatform(IProject project, Platforms service, Guid guid)
        {

            RegisteredPlatform platform;

            try
            {
                if (service.TryGetPlatform(guid, out platform))
                {
                    log.Info(Resources.log_build_interfaces);
                    var status = new OperationStatus();

                    return platform.Generate(project, status, BuildType.BuildAll) ?? new string[0];
                }

            }
            catch (InvalidOperationException ex)
            {
                // thrown by VFS when connection string is invalid
                // This occurs when deploying the dll to the database.
                if (ex.Source != "Sage.Platform.VirtualFileSystem" &&
                    ex.Source != "System.Data")
                    throw;
            }

            return new string[0];
        }

        /// <summary>
        /// Update the service references
        /// </summary>
        /// <param name="outputFiles"></param>
        static void OnBuildComplete(string[] outputFiles)
        {

            foreach (string s in outputFiles)
            {
                if (String.Equals(Path.GetFileName(s), "sage.entity.interfaces.dll", StringComparison.InvariantCultureIgnoreCase))
                {
                    
                    ReferenceAssembly(s);

                    break;
                }
            }
        }

        private static void ReferenceAssembly(string assemblyPath)
        {
            var service = ApplicationContext.Current.Services.Get<ITypeResolutionService>();
            service.ReferenceAssembly(AssemblyName.GetAssemblyName(assemblyPath));
        }


        /// <summary>
        /// Register the needed services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="project"></param>
        private static void RegisterNeededServices(ServiceCollection services, IProject project)
        {
            ServiceCollection rootServices = ApplicationContext.Current.RootWorkItem.Services;

            rootServices.Add<IProjectContextService>(new SimpleProjectContextService(project));
            rootServices.AddNew<ProjectLocalizationService>();
            rootServices.Add<IPortalDeploymentService>(new PortalDeploymentService());

            ITypeResolutionService resService = new TypeResolutionService();
            services.Add(typeof(ITypeResolutionService), resService);
            services.Add(typeof(ITypeDiscoveryService), resService);
        }

        static object lockObject = new object();

        private void DeployTarget(string targetName)
        {

            log.InfoFormat(Resources.log_begging_target_deployment, targetName);

            ServiceCollection services = ApplicationContext.Current.Services;
            IPortalDeploymentService service = services.Get<IPortalDeploymentService>() as IPortalDeploymentService;
            if (service != null)
            {

                Sage.Platform.Deployment.Deployment active = null;

                lock (lockObject)
                {
                    foreach (Sage.Platform.Deployment.Deployment deployment in service.Deployments)
                    {
                        if (string.Compare(deployment.DeploymentName, targetName, true) == 0)
                        {
                            active = deployment;
                            break;
                        }
                    }
                }

                if (active != null)
                {
                    RunDeploymentSync(active);
                }

            }

        }

        private void RunDeploymentSync(Sage.Platform.Deployment.Deployment deployment)
        {
            var wrapper = new DeploymentCallbackWrapper();

            try
            {
                log.Info(Resources.log_deployment_started);
                wrapper.DeployPortal(deployment);
                log.Info(Resources.log_deployment_ended);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Handles the deployment of a portal
        /// </summary>
        private void DeployPortal()
        {

            string portalName = Manifest.PortalName;

            CabApplicationPortal application = CabApplicationPortal.Get(portalName);

            if (application != null)
            {

                var deployment = new Sage.Platform.Deployment.Deployment
                {
                    DeploymentName = portalName
                };

                InitializeDeployment(deployment, application.Id.ToString());

                var wrapper = new DeploymentCallbackWrapper();
                try
                {

                    log.Info(Resources.log_deployment_started);
                    wrapper.DeployPortal(deployment);
                    log.Info(Resources.log_deployment_ended);

                    var callback = new PrecompileBuildManagerCallback();
                    var buildParameter = new ClientBuildManagerParameter
                    {
                        PrecompilationFlags = PrecompilationFlags.OverwriteTarget | PrecompilationFlags.Clean | PrecompilationFlags.CodeAnalysis
                    };


                    if (Manifest.Precompile)
                    {
                        log.Info(Resources.log_precompile_started);
                        DeploymentUtil.PrecompileSite(callback, buildParameter, Manifest.DeploymentPath, null, null, "/" + portalName, false);
                        log.Info(Resources.log_precompile_finished);
                    }

                }
                catch (Exception ex)
                {
                    log.Error(Resources.error_deploying_portal, ex);
                }
            }
        }

        /// <summary>
        /// Configures the deployment 
        /// </summary>
        /// <param name="deployment"></param>
        /// <param name="portalInstanceId"></param>
        private void InitializeDeployment(Sage.Platform.Deployment.Deployment deployment, string portalInstanceId)
        {
            // Configure the portal
            var portal = new DeploymentTargetPortal
            {
                PortalName = Manifest.PortalName,
                IsActive = true,
                InstanceId = portalInstanceId
            };

            // Configure the target
            var target = new DeploymentTarget("FS")
            {
                IsActive = true
            };

            target.ExtendedProperties.SetValue(DeploymentTargetConstants.DeploymentPath, Manifest.DeploymentPath);
            target.Portals.Add(portal);
            deployment.Targets.Add(target);
        }

        private static void InitializeVFS(string conString)
        {
            VFSQuery.ConfigureVFS(new VFSConfiguration(conString, false, Environment.MachineName, true, new TimeSpan(0, 2, 0)), true);
        }

        static OrmModel GetModel(IProject project)
        {
            return project.Models.Get<OrmModel>();
        }

        private void GenerateRulesConfiguration(IProject currentProject)
        {
            DynamicMethodConfiguration config = GetModel(currentProject).GenerateDynamicMethodConfiguration();

            if (!ConfigManager.IsConfigurationTypeRegistered(typeof(DynamicMethodConfiguration)))
            {
                ConfigManager.RegisterConfigurationType(
                    new ReflectionConfigurationTypeInfo(typeof(DynamicMethodConfiguration))
                    {
                        ConfigurationSourceType = typeof(AppDataConfigurationSource)
                    });
            }

            ConfigManager.WriteConfiguration(config);

            var ser = new XmlSerializer(typeof(DynamicMethodConfiguration));
            IDriveInfo dstDrive = currentProject.Drive;
            string commonPath = String.Format("{0}\\webroot\\common", currentProject.CommonPaths["DEPLOYMENTPATH"]);
            IDirectoryInfo dstDir = dstDrive.GetDirectoryInfo(commonPath);
            if (!dstDir.Exists)
                dstDir.Create();

            IFileInfo fi = dstDrive.GetFileInfo(Path.Combine(dstDir.Url, DynamicMethodLibraryHelper.CONFIG_FILE));
            using (TextWriter tw = fi.CreateText())
            {
                ser.Serialize(tw, config);
            }
        }

        

        public void GenerateCodeSnippetLibraries(IProject activeProject)
        {
            if (activeProject != null)
            {

                var mgr = new CodeSnippetManager(activeProject);
                mgr.BuildBasePath = Manifest.OutputPath;
                mgr.BuildAllLibraries(true);

                #region Deploy Generated Assemblies

                IDriveInfo di = Sage.Platform.FileSystem.FileSystem.GetDrive(mgr.BuildBasePath);
                IDriveInfo dstDrive = activeProject.Drive;
                IDirectoryInfo dstDir = dstDrive.GetDirectoryInfo(activeProject.CommonPaths["DEPLOYMENTPATH"] + "\\webroot\\common\\bin");

                if (!dstDir.Exists)
                    dstDir.Create();

                foreach (var library in mgr.Libraries)
                {
                    string assembly = library.AssemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase)
                                          ? library.AssemblyName : library.AssemblyName + ".dll";

                    IFileInfo src = di.GetFileInfo(Path.Combine(library.ProjectBuildOutputPath, assembly));
                    if (src.Exists)
                    {
                        IFileInfo dst = dstDrive.GetFileInfo(Path.Combine(dstDir.Url, assembly));
                        FSFile.Copy(src, dst, true);

                        assembly = Path.ChangeExtension(assembly, ".pdb");
                        src = di.GetFileInfo(Path.Combine(library.ProjectBuildOutputPath, assembly));
                        if (src.Exists)
                        {
                            dst = dstDrive.GetFileInfo(Path.Combine(dstDir.Url, assembly));
                            FSFile.Copy(src, dst, true);
                        }
                    }
                }

                #endregion Deploy Generated Assemblies
            }
        }




    }
}
