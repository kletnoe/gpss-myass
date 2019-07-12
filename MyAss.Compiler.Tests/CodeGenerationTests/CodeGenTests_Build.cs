using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Compiler.CodeGeneration;
using NUnit.Framework;
using MyAss.Framework;
using MyAss.Compiler;
using MyAss.Compiler.AST;

namespace MyAss.Compiler.Tests.CodeGenerationTests
{
    [TestFixture]
    [Category("CodeGenTests_Build")]
    public class CodeGenTests_Build
    {
        [Test]
        [Ignore("")]
        public void DirectSna()
        {
            string input = @"Some Q$Tail";
            CommonCode(input);
        }

        [Test]
        [Ignore("")]
        public void SingleVerb()
        {
            string input = @"Generate 10, 15, 13+1/2+1-1";
            CommonCode(input);
        }

        [Test]
        public void PrecedureCall()
        {
            string input = @"GENERATE (Exponential(1,0,1/2))";
            CommonCode(input);
        }

        [Test]
        public void PrecedureCallComplex()
        {
            string input = @"GENERATE (Exponential(1,0,(1+0.4)/2))";
            CommonCode(input);
        }

        [Test]
        public void VerbWithLiteral()
        {
            string input = @"TEST L Q$Tail,20,GoAway";
            CommonCode(input);
        }

        [Test]
        public void JustVerbName()
        {
            string input = @"TERMINATE";
            CommonCode(input);
        }

        [Test]
        public void JustVerbNameLF()
        {
            string input = @"
TERMINATE;comment
  ";
            CommonCode(input);
        }

        [Test]
        public void JustVerbNameComment()
        {
            string input = @"TERMINATE;comment";
            CommonCode(input);
        }

        [Test]
        public void VerbWithName()
        {
            string input = @"SAVEVALUE GenerateCounter,1";
            CommonCode(input);
        }

        [Test]
        public void SnaCall()
        {
            string input = @"SAVEVALUE GenerateCounter,X$GenerateCounter+1";
            CommonCode(input);
        }

        private static void CommonCode(string input)
        {
            input = Defaults.DefUsing + input;

            AssemblyCompiler compiler = new AssemblyCompiler(input, true);
            compiler.Compile(true);

            Assert.Pass(GenerationUtils.PrintCodeObject(compiler.CompileUnit));
        }
    }
}
