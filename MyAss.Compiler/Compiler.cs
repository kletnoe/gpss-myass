using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using MyAss.Compiler.Metadata;
using MyAss.Compiler;
using MyAss.Compiler.AST;
using MyAss.Compiler.CodeGeneration;

namespace MyAss.Compiler
{
    public class AssemblyCompiler
    {
        private const string DefaultOutputDir = @"e:\temp\";
        private const string DefaultOutputExePath = DefaultOutputDir + "TheModel.exe";

        public string ModelSources { get; private set; }
        public List<string> ReferencedDlls { get; private set; }
        public string OutputExePath { get; private set; }

        public ASTModel ASTModel { get; private set; }
        public MetadataRetriever MetadataRetriever { get; private set; }
        public CodeCompileUnit CompileUnit { get; private set; }
        public Assembly Assembly { get; private set; }

        public static IReadOnlyList<string> DefaultRefs
        {
            get
            {
                return new List<string>()
                {
                    "MyAss.Utilities.dll",
                    "MyAss.Framework.dll",
                    "MyAss.Framework.BuiltIn.dll",
                    "MyAss.Framework.Procedures.dll",
                    "MyAss.Framework.TablePackage.dll",
                    "MyCustomLibrary.dll",
                };
            }
        }

        public static IReadOnlyList<string> DefaultNamespaces
        {
            get
            {
                return new List<string>()
                {
                    "MyAss.Framework.BuiltIn.Blocks",
                    "MyAss.Framework.BuiltIn.Commands"
                };
            }
        }

        public static IReadOnlyList<string> DefaultTypes
        {
            get
            {
                return new List<string>()
                {
                    "MyAss.Framework.BuiltIn.SNA.SavevalueSNA",
                    "MyAss.Framework.BuiltIn.SNA.QueueSNA",
                    "MyAss.Framework.Procedures.Distributions"
                };
            }
        }

        public AssemblyCompiler(string modelSources, bool useDefaultDlls)
        {
            this.ModelSources = modelSources;
            this.OutputExePath = DefaultOutputExePath;
            this.ReferencedDlls = new List<string>();

            if (useDefaultDlls)
            {
                this.ReferencedDlls.AddRange(DefaultRefs);
            }
        }

        public AssemblyCompiler(FileInfo modelFile, List<FileInfo> libraries)
        {
            this.ModelSources = File.ReadAllText(modelFile.FullName);
            this.OutputExePath = modelFile.FullName + ".exe";
            this.ReferencedDlls = libraries.Select(x => x.FullName).ToList();
        }


        public AssemblyCompiler(string modelSources, List<string> referencedDlls, string outputExePath)
        {
            this.ModelSources = modelSources;
            this.ReferencedDlls = referencedDlls;
            this.OutputExePath = outputExePath;
        }

        public void Compile(bool inMemory)
        {
            Parser parser = new Parser(new Scanner(new CharSourceTokenizer(this.ModelSources)), this.ReferencedDlls);
            this.ASTModel = parser.Model;
            this.MetadataRetriever = parser.MetadataRetriever;

            CompileUnitConstructor compileUnitConstructor = new CompileUnitConstructor(this.ASTModel, this.MetadataRetriever);
            this.CompileUnit = compileUnitConstructor.CompileUnit;

            CompileAssembly(inMemory);
        }

        public void RunAssembly()
        {
            this.Assembly.EntryPoint.Invoke(null, null);
        }

        private void CompileAssembly(bool inMemory)
        {
            CompilerParameters compilerParams = new CompilerParameters();
            compilerParams.GenerateInMemory = inMemory;
            compilerParams.GenerateExecutable = true;
            compilerParams.OutputAssembly = inMemory ? null : this.OutputExePath;

            compilerParams.ReferencedAssemblies.Add("System.dll");
            compilerParams.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            compilerParams.ReferencedAssemblies.Add("System.Core.dll");
            compilerParams.ReferencedAssemblies.AddRange(this.ReferencedDlls.ToArray());

            //compilerParams.TempFiles = new TempFileCollection(DefaultOutputDir, true);

            //compilerParams.CompilerOptions = "/optimize-";

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerResults results = provider.CompileAssemblyFromDom(compilerParams, this.CompileUnit);

            if (results.Errors.Count != 0)
            {
                throw this.ResultErrorsToException(results.Errors);
            }

            this.Assembly = results.CompiledAssembly;
        }

        private AggregateException ResultErrorsToException(CompilerErrorCollection errors)
        {
            List<CompilerException> exceptions = new List<CompilerException>();
            for (int i = 0; i < errors.Count; i++)
            {
                exceptions.Add(new CompilerException(errors[i].ErrorText));
            }

            return new AggregateException(exceptions);
        }
    }
}
