using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.Commands;

namespace MyAss.Compiler.Metadata
{
    public class MetadataRetriever_v2
    {
        private HashSet<string> assemblyPaths;
        private HashSet<string> referencedNamespaces;
        private HashSet<string> referencedTypes;

        private List<Assembly> assemblies;
        private List<Type> verbs;
        private List<MethodInfo> procedures;

        public ISet<string> AsssemblyPaths
        {
            get
            {
                return this.assemblyPaths;
            }
        }

        public IReadOnlyList<Type> Verbs
        {
            get
            {
                return this.verbs;
            }
        }

        public IReadOnlyList<MethodInfo> Procedures
        {
            get
            {
                return this.procedures;
            }
        }

        private MetadataRetriever_v2()
        {
            this.assemblyPaths = new HashSet<string>();
            this.referencedNamespaces = new HashSet<string>();
            this.referencedTypes = new HashSet<string>();

            this.assemblies = new List<Assembly>();
            this.verbs = new List<Type>();
            this.procedures = new List<MethodInfo>();
        }

        public MetadataRetriever_v2(HashSet<string> assemblyPaths, HashSet<string> referencedNamespaces, HashSet<string> referencedTypes)
            :this()
        {
            this.assemblyPaths = assemblyPaths;
            this.referencedNamespaces = referencedNamespaces;
            this.referencedTypes = referencedTypes;

            foreach (var assemblyPath in assemblyPaths)
            {
                assemblies.Add(Assembly.LoadFrom(assemblyPath));
            }

            this.verbs.AddRange(this.GetAssignableTypes(typeof(ICommand)));
            this.verbs.AddRange(this.GetAssignableTypes(typeof(IBlock)));
            this.procedures = this.GetProcedures();
        }

        private List<Type> GetAssignableTypes(Type parentType)
        {
            List<Type> types = new List<Type>();

            foreach (var assembly in assemblies)
            {
                foreach (var referencedNamespace in referencedNamespaces)
                {
                    types.AddRange(assembly.GetTypes().Where((x => x.Namespace == referencedNamespace 
                        && parentType.IsAssignableFrom(x)
                        && !x.IsInterface 
                        && !x.IsAbstract)));
                }
            }

            return types;
        }

        private List<MethodInfo> GetProcedures()
        {
            List<MethodInfo> procedures = new List<MethodInfo>();

            foreach (var assembly in assemblies)
            {
                foreach (var referencedeType in referencedTypes)
                {
                    Type type = assembly.GetType(referencedeType);

                    if (type != null)
                    {
                        procedures.AddRange(type.GetMethods().Where(x => x.IsStatic));
                    }
                }
            }

            return procedures;
        }

        public bool IsVerb(string verbName)
        {
            if (verbName.Contains('.'))
            {
                return this.verbs.Where(x => String.Equals(x.FullName, verbName, StringComparison.InvariantCultureIgnoreCase)).Any();
            }
            else
            {
                return this.verbs.Where(x => String.Equals(x.Name, verbName, StringComparison.InvariantCultureIgnoreCase)).Any();
            }
        }

        public Type GetVerb(string verbName)
        {
            IEnumerable<Type> verbsFound;

            if (verbName.Contains('.'))
            {
                verbsFound = this.verbs.Where(x => String.Equals(x.FullName, verbName, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                verbsFound = this.verbs.Where(x => String.Equals(x.Name, verbName, StringComparison.InvariantCultureIgnoreCase));
            }

            if (verbsFound.Count() == 1)
            {
                return verbsFound.First();
            }
            else if(verbsFound.Count() == 0)
            {
                throw new Exception("Verb " + verbName + " not found by name");
            }
            else
            {
                throw new Exception("Found " + verbsFound.Count() + " verbs with name " + verbName);
            }
        }

        public MethodInfo GetProcedure(string procedureName)
        {
            IEnumerable<MethodInfo> procedures = this.procedures.Where(x => String.Equals(x.Name, procedureName, StringComparison.InvariantCultureIgnoreCase));
            if (procedures.Count() == 1)
            {
                return procedures.First();
            }
            else if (procedures.Count() == 0)
            {
                throw new Exception("Procedure " + procedureName + " not found by name");
            }
            else
            {
                throw new Exception("Found " + procedures.Count() + " procedures with name " + procedureName);
            }
        }
    }
}
