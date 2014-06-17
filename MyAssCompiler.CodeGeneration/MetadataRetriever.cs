using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MyAssCompiler.CodeGeneration
{
    public class MetadataRetriever
    {
        public string AssemblyName { get { return "MyAssFramework"; } }
        public string AssemblyDllName { get { return this.AssemblyName + ".dll"; } }
        
        public IEnumerable<Type> GetMyAssFrameworkBlockTypes()
        {
            return Assembly.LoadFrom(this.AssemblyDllName).GetTypes().Where(x => !x.IsInterface && !x.IsAbstract)
                .Where(x => x.Namespace == this.AssemblyName + ".Blocks");
        }

        public IEnumerable<Type> GetMyAssFrameworkCommandTypes()
        {
            return Assembly.LoadFrom(this.AssemblyDllName).GetTypes().Where(x => !x.IsInterface && !x.IsAbstract)
                .Where(x => x.Namespace == this.AssemblyName + ".Commands");
        }

        public Type GetMyAssFrameworkSNAType()
        {
            return Assembly.LoadFrom(this.AssemblyDllName).GetType(this.AssemblyName + ".SNA.SNA");
        }

        public IEnumerable<MethodInfo> GetMyAssFrameworkSNAMethods()
        {
            return this.GetMyAssFrameworkSNAType().GetMethods().Where(x => x.IsStatic);
        }

        public IEnumerable<string> GetAllDefinitions()
        {
            return this.GetMyAssFrameworkBlockTypes().Select(x => x.Name)
                .Concat(this.GetMyAssFrameworkCommandTypes().Select(x => x.Name))
                .Concat(this.GetMyAssFrameworkSNAMethods().Select(x => x.Name))
                ;
        }


        /////

        private const string builtinTypesAssemblyName = "MyAssFramework.dll";
        private const string builtinSnaTypeName = "MyAssFramework.SNA.SNA";

        public static MethodInfo GetSnaMethod(string name)
        {
            return GetBuiltinSnaMethod(name);
        }

        // Always builtin for now
        private static MethodInfo GetBuiltinSnaMethod(string name)
        {
            return GetBuiltinSnaType().GetMethod(name);
        }

        public static Type GetBuiltinSnaType()
        {
            return Assembly.LoadFrom(builtinTypesAssemblyName).GetType(builtinSnaTypeName);
        }
    }
}
