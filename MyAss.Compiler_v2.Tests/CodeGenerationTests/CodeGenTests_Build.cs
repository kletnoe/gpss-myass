using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Compiler_v2.CodeGeneration;
using NUnit.Framework;
using MyAss.Framework;
using MyAss.Compiler;

namespace MyAss.Compiler_v2.Tests.CodeGenerationTests
{
    [TestFixture]
    [Category("CodeGenTests_v2_Build")]
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
        [Ignore]
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

        [Test]
        
        public void Model()
        {
            string input = @"
Server STORAGE 3

	START 1000

	GENERATE (Exponential(1,0,1/2))
	SAVEVALUE GenerateCounter,X$GenerateCounter+1

	;TEST L Q$Tail,20,GoAway		;Jump if in Stack >20
	QUEUE Tail
	ENTER Server,1
	DEPART Tail
	ADVANCE (Exponential(2,0,1/0.2))
	LEAVE Server,1

	SAVEVALUE RejetionProb,(X$RejectCounter/X$GenerateCounter)
	TERMINATE 1


GoAway	SAVEVALUE RejectCounter,X$RejectCounter+1
	TERMINATE 		;Delete rejected.

";
            CommonCode(input);
        }

        private static void CommonCode(string input)
        {
            Parser_v2 parser = new Parser_v2(new Scanner(new StringCharSource(input)));
            CodeDomGenerationVisitor vis = new CodeDomGenerationVisitor(parser);

            CodeCompileUnit assembly = vis.VisitAll();
            Compilation.CompileAssembly(assembly, false);

            Assert.Pass(Compilation.PrintCodeObject(assembly));
        }
    }
}
