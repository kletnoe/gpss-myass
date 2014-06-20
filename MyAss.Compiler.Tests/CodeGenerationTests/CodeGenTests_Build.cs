using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Compiler.CodeGeneration;
using NUnit.Framework;
using MyAss.Framework;

namespace MyAss.Compiler.Tests.CodeGenerationTests
{
    [TestFixture]
    [Category("CodeGenTests_Build")]
    public class CodeGenTests_Build
    {
        [Test]
        [Ignore]
        public void DirectSna()
        {
            string input = @"Some Q$Tail";

            CommonCode(input);
        }

        [Test]
        public void SingleVerb()
        {
            string input = @"Generate 10, 15,, 13+1/2+1-1";

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
