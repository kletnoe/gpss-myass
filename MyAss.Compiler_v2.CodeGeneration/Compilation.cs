using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CSharp;

namespace MyAss.Compiler_v2.CodeGeneration
{
    public static class Compilation
    {
        private const string outputPath = @"c:\temp\HelloWorld.dll";

        public static void CompileAssembly(CodeCompileUnit compileUnit, bool InMemory)
        {
            CompilerParameters compilerParams = new CompilerParameters()
            {
                GenerateInMemory = InMemory,
                GenerateExecutable = false,
                OutputAssembly = InMemory ? null : outputPath
            };

            compilerParams.ReferencedAssemblies.AddRange(new string[]{
                "MyAss.Framework_v2.dll",
                "MyAss.Framework_v2.BuiltIn.dll",
                "MyAss.Framework.Procedures.dll"});

            compilerParams.TempFiles = new TempFileCollection(@"c:\temp\", true);

            compilerParams.CompilerOptions = "/optimize-";

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerResults results = provider.CompileAssemblyFromDom(compilerParams, compileUnit);

            if (results.Errors.Count != 0)
            {
                throw ResultErrorsToException(results.Errors);
            }
        }

        public static string PrintCodeObject(CodeCompileUnit compileUnit)
        {
            CodeGeneratorOptions options = new CodeGeneratorOptions()
            {

            };

            StringWriter writer = new StringWriter();

            CSharpCodeProvider provider = new CSharpCodeProvider();
            provider.GenerateCodeFromCompileUnit(compileUnit, writer, options);

            writer.Flush();
            return writer.ToString();
        }

        private static AggregateException ResultErrorsToException(CompilerErrorCollection errors)
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
