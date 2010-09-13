using Sage.Platform.WebPortal.Design;

namespace QuickDeploymentModule
{
    public class GeneratorProviderBase
    {
        protected PortalApplication _portal;

        public GeneratorProviderBase(PortalApplication portal)
        {
            _portal = portal;
        }

        protected string GetBaseOutputCacheFolder()
        {
            return string.Format("{0}\\deployment\\OutputCache\\Portals\\{1}\\",
                                 _portal.FilePath.DriveInfo.RootDirectory.FullName, _portal.PortalAlias);
        }
    }
}