using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sage.Platform.Application;
using Sage.Platform.Application.UI;
using Sage.Platform.Deployment;
using Sage.Platform.FileSystem.Interfaces;
using Sage.Platform.Projects.Interfaces;

namespace QuickDeploymentModule
{
    public class ModuleInit : ModuleInit<UIWorkItem>, IModuleConfigurationProvider
    {
        private IProjectContextService _projectContext;
        private IFileSystemWatcher _fileWatcher;
        private List<string> _filesTouchedSinceLastQuickDeploy; //only tracks current AA session

        protected override void Load()
        {
            base.Load();

            _filesTouchedSinceLastQuickDeploy = new List<string>();
            ModuleWorkItem.State["FilesChangedSinceLastQuickDeploy"] = _filesTouchedSinceLastQuickDeploy;            

            _projectContext = ParentWorkItem.Services.Get<IProjectContextService>(true);
            _projectContext.ActiveProjectChanged += (sender, args) => SetupFileSystemWatcher();
            SetupFileSystemWatcher();
        }

        private void SetupFileSystemWatcher()
        {
            if (_fileWatcher != null)
                _fileWatcher.Dispose();

            if (_projectContext.ActiveProject == null)
                return;

            var fileNotify = _projectContext.ActiveProject.Drive.RootDirectory as IFileChangeNotify;
            if (fileNotify == null)
                return;

            _fileWatcher = fileNotify.CreateWatcher();
            _fileWatcher.Changed += (sender, args) => HandleFileChanged(args.FullPath);
            _fileWatcher.Created += (sender, args) => HandleFileChanged(args.FullPath);
            _fileWatcher.Deleted += (sender, args) => HandleFileChanged(args.FullPath);
            _fileWatcher.IncludeSubdirectories = true;
            _fileWatcher.EnableRaisingEvents = true;

            _filesTouchedSinceLastQuickDeploy.Clear();
            ModuleWorkItem.State["LastQuickDeployedPortal"] = null;
        }

        private void HandleFileChanged(string url)
        {
            if (!_filesTouchedSinceLastQuickDeploy.Contains(url))
                _filesTouchedSinceLastQuickDeploy.Add(url);
        }

        public ModuleConfiguration GetConfiguration()
        {
            return ModuleConfiguration.LoadFromResource("QuickDeploymentModule.ModuleConfig.xml", GetType().Assembly);
        }

        [CommandHandler("cmd://DeployTargetPortal")]
        public void DeployToTargetPortal(object sender, EventArgs e)
        {
            var deploymentSvc = ParentWorkItem.Services.Get<IPortalDeploymentService>(true);
            Deployment deployment = deploymentSvc.DebugDeployment;

            if (deployment == null)
                throw new Exception("No debug deployment found.");

            TargetPortalInfo selectedTargetPortal = null;
            using (var dlg = new SelectTargetPortalDialog(deployment))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    selectedTargetPortal = dlg.SelectedTargetPortal;
                }
            }

            if (selectedTargetPortal == null)
                return;

            var quickDeploy = new QuickDeploy(selectedTargetPortal.Target, selectedTargetPortal.TargetPortal, ModuleWorkItem);
            quickDeploy.Deploy();
        }

        [CommandHandler("cmd://QuickDeploy/ResetFileWatcher")]
        public void ResetQuickDeployFileWatcher(object sender, EventArgs e)
        {
            _filesTouchedSinceLastQuickDeploy.Clear();
        }
    }
}
