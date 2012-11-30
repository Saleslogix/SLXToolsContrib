using Sage.Platform.WebPortal.Design;

namespace QuickDeploymentModule
{
    public class GeneratorProviderBase
    {
        protected CabApplicationPortal _portal;

        public GeneratorProviderBase(CabApplicationPortal portal)
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