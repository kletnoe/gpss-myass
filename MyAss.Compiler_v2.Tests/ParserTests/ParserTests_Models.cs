using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler;
using MyAss.Compiler_v2;
using MyAss.Compiler_v2.AST;
using NUnit.Framework;

namespace MyAss.Compiler_v2.Tests.ParserTests
{
    [TestFixture]
    [Category("ParserTests_v2_Models")]
    public class ParserTests_Models
    {
        [Test]
        public void Block_DirSna2()
        {
            string input = @"
Server STORAGE 3

	START 1000

	GENERATE (Exponential(1,0,1/2))
	SAVEVALUE GenerateCounter,X$GenerateCounter+1

	TEST L Q$Tail,20,GoAway		;Jump if in Stack >20
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
            Assert.Pass(this.RunModel(input).ToString());
        }

        private IASTNode RunModel(string input)
        {
            Parser_v2 parser = new Parser_v2(new Scanner(new StringCharSource(input)));
            ASTModel model = parser.Model;
            return model;
        }
    }
}
