using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler.CodeGeneration;
using NUnit.Framework;
using MyAssFramework;

namespace MyAssCompiler.Tests.CodeGenerationTests
{
    [TestFixture]
    public class CodeGenTests_Build
    {
        [Test]
        public void DirectSna()
        {
            string input = @"Some Q$Tail";

            CommonCode(input);
        }

        private static void CommonCode(string input)
        {
            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            vis.VisitAll();
            CodeCompileUnit assembly = vis.CreateAssembly();
            Compilation.CompileAssembly(assembly, true);

            Assert.Pass(Compilation.PrintCodeObject(assembly));
        }
    }
}
