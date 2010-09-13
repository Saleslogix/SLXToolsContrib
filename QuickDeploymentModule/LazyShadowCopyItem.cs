using System;
using System.IO;
using Sage.Platform.FileSystem;
using Sage.Platform.WebPortal.Design;

namespace QuickDeploymentModule
{
    public class LazyShadowCopyItem : IShadowCopyItem
    {
        public void Dispose()
        {
        }

        public LazyShadowCopyItem(string fullPath, string url)
        {
            FullPath = fullPath;
            Url = url;

            var file = FileSystem.GetFileInfo(FullPath);
            Size = file.Length;
            ModifiedDate = file.LastWriteTimeUtc;
        }

        public string FullPath { get; private set; }

        public long Size { get; private set; }

        public DateTime ModifiedDate { get; private set; }

        public string Url { get; private set; }

        public Stream Data
        {
            get { throw new NotImplementedException(); }
        }

        public string Hash
        {
            get { throw new NotImplementedException(); }
        }
    }
}