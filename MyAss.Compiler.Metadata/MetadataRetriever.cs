using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.Commands;

namespace MyAss.Compiler.Metadata
{
    public class MetadataRetriever
    {
        private List<string> assemblyPaths;
        private List<Assembly> assemblies;
        private List<TypeInfo> verbTypes;

        public IReadOnlyCollection<string> AsssemblyPaths
        {
            get
            {
                return this.assemblyPaths;
            }
        }

        public IReadOnlyCollection<Assembly> Asssemblies
        {
            get
            {
                return this.assemblies;
            }
        }

        public IReadOnlyCollection<TypeInfo> VerbTypes
        {
            get
            {
                return this.verbTypes;
            }
        }

        public MetadataRetriever(List<string> assemblyPaths)
        {
            this.assemblies = new List<Assembly>();
            //this.assemblies.Add(GetMscorlib());

            List<string> assemblyPathsDistinct = assemblyPaths.Distinct().ToList();
            this.assemblyPaths = assemblyPathsDistinct;

            foreach (var assemblyPath in assemblyPathsDistinct)
            {
                assemblies.Add(Assembly.LoadFrom(assemblyPath));
            }

            this.verbTypes = new List<TypeInfo>();
            this.verbTypes.AddRange(assemblies.SelectMany(t => t.DefinedTypes
                    .Where(x =>
                            (typeof(AnyBlock).IsAssignableFrom(x)
                                || typeof(AnyCommand).IsAssignableFrom(x))
                            && !x.IsInterface
                            && !x.IsAbstract
                    )
                )
            );
        }

        private static Assembly GetMscorlib()
        {
            return typeof(string).Assembly;
        }

        public bool IsVerbName(string verbFullName)
        {
            Tuple<string, string> splitted = this.SplitByLastPeriod(verbFullName);
            return this.IsVerbName(new List<string>() { splitted.Item1 }, splitted.Item2);
        }

        public bool IsVerbName(List<string> namespaceNames, string verbName)
        {
            IList<TypeInfo> verbsFound = this.FindVerbsByName(namespaceNames, verbName);

            if (verbsFound.Count() > 1)
            {
                throw new Exception("Found " + verbsFound.Count() + " verbs with name " + verbName);
            }
            else
            {
                return verbsFound.Any();
            }
        }

        public string ResolveVerbName(string verbFullName)
        {
            Tuple<string, string> splitted = this.SplitByLastPeriod(verbFullName);
            return this.ResolveVerbName(new List<string>() { splitted.Item1 }, splitted.Item2);
        }

        public string ResolveVerbName(List<string> namespaceNames, string verbName)
        {
            IList<TypeInfo> verbsFound = this.FindVerbsByName(namespaceNames, verbName);

            if (verbsFound.Count() == 1)
            {
                return verbsFound.First().FullName;
            }
            else if (verbsFound.Count() == 0)
            {
                throw new Exception("Verb " + verbName + " not found by name");
            }
            else
            {
                throw new Exception("Found " + verbsFound.Count() + " verbs with name " + verbName);
            }
        }

        private IList<TypeInfo> FindVerbsByName(List<string> namespaceNames, string verbName)
        {
            List<TypeInfo> verbsFound = new List<TypeInfo>();

            foreach (var namespaceName in namespaceNames)
            {
                verbsFound.AddRange(this.FindVerbsByName(namespaceName, verbName));
            }

            return verbsFound;
        }

        private IList<TypeInfo> FindVerbsByName(string namespaceName, string verbName)
        {
            List<TypeInfo> verbsFound = new List<TypeInfo>();

            verbsFound.AddRange(verbTypes.Where(x =>
                x.Namespace == namespaceName
                && String.Equals(x.Name, verbName, StringComparison.InvariantCultureIgnoreCase)
            ));

            return verbsFound;
        }

        public string ResolveFunctionName(string functionFullName)
        {
            Tuple<string, string> splitted = this.SplitByLastPeriod(functionFullName);
            return this.ResolveFunctionName(new List<string>() { splitted.Item1 }, splitted.Item2);
        }

        public string ResolveFunctionName(List<string> referencedTypes, string functionName)
        {
            IList<MethodInfo> functionsFound = this.FindFunctionsByName(referencedTypes, functionName);

            if (functionsFound.Count() == 1)
            {
                var function = functionsFound.First();
                return function.DeclaringType + "." + function.Name;
            }
            else if (functionsFound.Count() == 0)
            {
                throw new Exception("Function " + functionName + " not found by name");
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                foreach (var func in functionsFound)
                {
                    sb.AppendLine(func.DeclaringType.FullName);
                }
                throw new Exception("Found " + functionsFound.Count() + " functions with name " + functionName + sb.ToString());
            }
        }

        private IList<MethodInfo> FindFunctionsByName(List<string> referencedTypes, string functionName)
        {
            List<MethodInfo> procedures = new List<MethodInfo>();

            foreach (var assembly in this.assemblies)
            {
                foreach (var referencedeType in referencedTypes)
                {
                    Type type = assembly.GetType(referencedeType);

                    if (type != null)
                    {
                        procedures.AddRange(type.GetMethods().Where(x => 
                            x.IsStatic
                            && !x.IsGenericMethod
                            //&& x.GetParameters().ToList().All(p => p.ParameterType == typeof(double))
                            && String.Equals(x.Name, functionName, StringComparison.InvariantCultureIgnoreCase)
                        ));
                    }
                }
            }

            return procedures;
        }

        private IList<MethodInfo> FindFunctionsByName(string referencedType, string functionName)
        {
            List<MethodInfo> procedures = new List<MethodInfo>();

            foreach (var assembly in assemblies)
            {
                Type type = assembly.GetType(referencedType);

                if (type != null)
                {
                    procedures.AddRange(type.GetMethods().Where(x =>
                        x.IsStatic
                        && String.Equals(x.Name, functionName, StringComparison.InvariantCultureIgnoreCase)
                    ));
                }
            }

            return procedures;
        }

        public TypeInfo GetVerbType(string verbFullName)
        {
            IEnumerable<TypeInfo> verbsFound = this.verbTypes.Where(x => x.FullName == verbFullName);

            if (verbsFound.Count() == 1)
            {
                return verbsFound.First();
            }
            else if (verbsFound.Count() == 0)
            {
                throw new Exception("Verb " + verbFullName + " not found by FullName");
            }
            else
            {
                throw new Exception("Found " + verbsFound.Count() + " verbs with FullName " + verbFullName);
            }
        }

        public MethodInfo GetFunctionMethod(string functionName)
        {
            Tuple<string, string> splitted = this.SplitByLastPeriod(functionName);

            IEnumerable<MethodInfo> functions = this.FindFunctionsByName(splitted.Item1, splitted.Item2);

            if (functions.Count() == 1)
            {
                return functions.First();
            }
            else if (functions.Count() == 0)
            {
                throw new Exception("Procedure " + functionName + " not found by name");
            }
            else
            {
                throw new Exception("Found " + functions.Count() + " procedures with name " + functionName);
            }
        }

        private Tuple<string, string> SplitByLastPeriod(string source)
        {
            int lastPeriodIndex = source.LastIndexOf('.');
            string firstPart = source.Substring(0, lastPeriodIndex);
            string secondPart = source.Substring(lastPeriodIndex + 1);

            return new Tuple<string, string>(firstPart, secondPart);
        }
    }
}
