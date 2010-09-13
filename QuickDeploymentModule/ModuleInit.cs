using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sage.Platform.Application;
using Sage.Platform.Application.UI;
using Sage.Platform.Deployment;

namespace QuickDeploymentModule
{
    public class ModuleInit : ModuleInit<UIWorkItem>, IModuleConfigurationProvider
    {
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

            var quickDeploy = new QuickDeploy(selectedTargetPortal.Target, selectedTargetPortal.TargetPortal);
            quickDeploy.Deploy();
        }
    }
}
