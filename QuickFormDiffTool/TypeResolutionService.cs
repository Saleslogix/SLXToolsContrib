using System;
using System.ComponentModel.Design;
using System.Reflection;
using Sage.Platform.Extensibility;

namespace QuickFormDiff
{
    internal class TypeResolutionService : ITypeResolutionService
    {
        private Assembly _entitiesInterfaceAssembly = null;

        #region ITypeResolutionService Members

        public Assembly GetAssembly(AssemblyName name, bool throwOnError)
        {
            throw new NotImplementedException();
        }

        public Assembly GetAssembly(AssemblyName name)
        {
            throw new NotImplementedException();
        }

        public string GetPathOfAssembly(AssemblyName name)
        {
            throw new NotImplementedException();
        }

        public Type GetType(string name, bool throwOnError, bool ignoreCase)
        {
            if (TypeNameIsInEntitiesInterfaceDll(name))
            {
                Type foundType = GetTypeFromEntitiesInterfaceDll(name);
                if (foundType != null)
                    return foundType;
            }
            return Type.GetType(name, throwOnError, ignoreCase);
        }

        public Type GetType(string name, bool throwOnError)
        {
            if (TypeNameIsInEntitiesInterfaceDll(name))
            {
                Type foundType = GetTypeFromEntitiesInterfaceDll(name);
                if (foundType != null)
                    return foundType;
            }
            return Type.GetType(name, throwOnError);
        }

        public Type GetType(string name)
        {
            if (TypeNameIsInEntitiesInterfaceDll(name))
            {
                Type foundType = GetTypeFromEntitiesInterfaceDll(name);
                if (foundType != null)
                    return foundType;
            }
            return Type.GetType(name);
        }

        public void ReferenceAssembly(AssemblyName name)
        {
            throw new NotImplementedException("ReferenceAssembly is not supported on this TypeResolutionService");
        }

        private bool TypeNameIsInEntitiesInterfaceDll(string typeName)
        {
            return typeName.ToLower().Contains("sage.entity.interfaces");
        }

        private Type GetTypeFromEntitiesInterfaceDll(string typeName)
        {
            try
            {
                Assembly assembly = GetEntitiesInterfaceAssembly();

                int commaPosition = typeName.IndexOf(',');
                string unqualifiedTypeName = typeName.Substring(0, commaPosition);
                return assembly.GetType(unqualifiedTypeName, true);
            }
            catch(Exception)
            {
                return null;
            }
        }
        
        private Assembly GetEntitiesInterfaceAssembly()
        {
            if (_entitiesInterfaceAssembly == null)
            {
                string assemblyPath = BuildSettings.CurrentBuildSettings.SolutionFolder + "\\Interfaces\\Bin\\";
                _entitiesInterfaceAssembly = Assembly.LoadFrom(assemblyPath + "Sage.Entity.interfaces.dll");
            }

            return _entitiesInterfaceAssembly;
        }
        #endregion
    }
}