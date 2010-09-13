using System.Linq;
using System.Windows.Forms;
using Sage.Platform.Deployment;

namespace QuickDeploymentModule
{
    public partial class SelectTargetPortalDialog : Form
    {
        private static DeploymentTargetPortal _lastDeployedPortal;

        public SelectTargetPortalDialog(Deployment deployment)
        {
            InitializeComponent();

            var targetPortals = deployment.Targets
                .SelectMany(target => target.Portals.Select(portal => new TargetPortalInfo(target, portal)))
                .ToList();

            cboTargetPortals.DataSource = targetPortals;

            //default to last deployed target portal
            if (_lastDeployedPortal != null)
            {
                var portalFromList = targetPortals.FirstOrDefault(portal => portal.TargetPortal == _lastDeployedPortal);
                if (portalFromList != null)
                    cboTargetPortals.SelectedItem = portalFromList;
            }
        }

        public TargetPortalInfo SelectedTargetPortal { get; private set; }

        private void SelectTargetPortalDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                SelectedTargetPortal = (cboTargetPortals.SelectedItem as TargetPortalInfo);
                _lastDeployedPortal = SelectedTargetPortal.TargetPortal;
            }
        }
    }

    public class TargetPortalInfo
    {
        public DeploymentTarget Target { get; set; }
        public DeploymentTargetPortal TargetPortal { get; set; }

        public TargetPortalInfo(DeploymentTarget target, DeploymentTargetPortal targetPortal)
        {
            Target = target;
            TargetPortal = targetPortal;
        }

        public override string ToString()
        {
            return string.Format("({0}) {1}", Target.TargetType, TargetPortal.PortalName);
        }
    }
}
