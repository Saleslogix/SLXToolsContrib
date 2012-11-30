using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Sage.Platform.IDEModule;

namespace QuickFormDiff
{
    public class SimpleProjectReferencesService : IProjectReferencesService
    {
        readonly Dictionary<object, IProjectReferences> _references;

        public SimpleProjectReferencesService()
        {
            _references = new Dictionary<object, IProjectReferences>();
        }

        #region IProjectReferencesService Members

        public IProjectReferences CreateProjectReferences(object context)
        {
            return _references[context] = Unity.Resolve<CodeEditorAssemblyReferences>();
        }

        public void UnregisterProjectReferences(object context)
        {
            if (_references.ContainsKey(context))
                _references.Remove(context);
        }

        public IProjectReferences GetReferences(object context)
        {
            IProjectReferences references;
            _references.TryGetValue(context, out references);
            return references;
        }

        #endregion

        [Dependency]
        public IUnityContainer Unity { get; set; }
    }
}
