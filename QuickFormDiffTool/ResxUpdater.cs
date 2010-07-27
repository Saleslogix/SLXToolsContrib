using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;

namespace QuickFormDiff
{
    public class ResxUpdater
    {
        private string _sourceResxPath;
        private IEnumerable<KeyValuePair<string, string>> _resourcesAdded;
        private IEnumerable<KeyValuePair<string, string>> _resourcesRemoved;
        private IEnumerable<ResourceDifference> _resourcesModified;

        public ResxUpdater(string sourceResxPath, IEnumerable<KeyValuePair<string, string>> resourcesAdded, IEnumerable<KeyValuePair<string, string>> resourcesRemoved, IEnumerable<ResourceDifference> resourcesModified)
        {
            _sourceResxPath = sourceResxPath;
            _resourcesAdded = resourcesAdded;
            _resourcesRemoved = resourcesRemoved;
            _resourcesModified = resourcesModified;
        }

        public void UpdateResxWithChanges(string newResxPath)
        {
            var newFile = new FileInfo(newResxPath);
            if (newFile.Exists)
                newFile.Delete();

            var addedKeys = _resourcesAdded.Select(pair => pair.Key);
            var removedKeys = _resourcesRemoved.Select(pair => pair.Key);

            using(var reader = new ResXResourceReader(_sourceResxPath))
            using(var writer = new ResXResourceWriter(newResxPath))
            {
                foreach (DictionaryEntry resourceEntry in reader)
                {
                    string currentKey = (string)resourceEntry.Key;
                    if (addedKeys.Contains(currentKey))
                        continue;
                    if (removedKeys.Contains(currentKey))
                        continue;

                    var modifiedResource = _resourcesModified.FirstOrDefault(mod => mod.ResourceName == currentKey);
                    if (modifiedResource != null)
                        writer.AddResource(currentKey, modifiedResource.NewValue);
                    else
                        writer.AddResource(currentKey, resourceEntry.Value);
                }

                _resourcesAdded.ForEach(res => writer.AddResource(res.Key, res.Value));
            }            
        }
    }
}