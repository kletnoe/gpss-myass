using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2;

namespace MyAss.Compiler.Metadata
{
    class MetadataRetriever_v3
    {
        private Type verbType = typeof(IVerb);

        private HashSet<string> assemblyPaths;
        private List<Assembly> assemblies;

        public MetadataRetriever_v3(HashSet<string> assemblyPaths)
        {
            this.assemblyPaths = assemblyPaths;
            this.assemblies = new List<Assembly>();

            foreach (var assemblyPath in assemblyPaths)
            {
                assemblies.Add(Assembly.LoadFrom(assemblyPath));
            }
        }

        public ClassMeta GetClassMeta(string fullName)
        {
            IEnumerable<TypeInfo> types = assemblies.SelectMany(t => t.DefinedTypes
                .Where(x => x.FullName == fullName
                    && this.verbType.IsAssignableFrom(x)
                    && !x.IsInterface
                    && !x.IsAbstract)
                );

            if (types.Count() == 1)
            {
                TypeInfo type = types.First();
                return new ClassMeta(type.AssemblyQualifiedName, type.Namespace, type.Name);
            }
            else
            {
                throw new Exception();
            }
        }

        public MethodMeta GetMethodMeta(string fullName)
        {
            return null;
        }
    }
}
