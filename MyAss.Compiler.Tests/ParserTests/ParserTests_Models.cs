using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler.AST;
using NUnit.Framework;

namespace MyAss.Compiler.Tests.ParserTests
{
    [TestFixture]
    [Category("ParserTests_Models")]
    public class ParserTests_Models
    {
        [Test]
        public void Block_DirSna2()
        {
            string input = @"
Server STORAGE 3

;InSystem TABLE MP$InSystemTime,0,4,20
;OnServer TABLE MP$OnServTime,0,2,20
;OnQueue TABLE MP$OnQueueTime,0,4,20


	;START 1000

	INITIAL X$RejectCounter,0
	INITIAL X$GenerateCounter,0
	INITIAL X$RejetionProb,0

	GENERATE (Exponential(1,0,1/2))
;		MARK InSystemTime
;		MARK OnQueueTime
	SAVEVALUE GenerateCounter,1

	TEST L Q$Tail,20,GoAway		;Jump if in Stack >20
	QUEUE Tail
	ENTER Server,1
	DEPART Tail
;		TABULATE OnQueue
;		MARK OnServTime
	ADVANCE (Exponential(2,0,1/0.2))
	LEAVE Server
;		TABULATE OnServer		
;		TABULATE InSystem

	SAVEVALUE RejetionProb,(X$RejectCounter/X$GenerateCounter)
	TERMINATE 1


GoAway	SAVEVALUE RejectCounter,1
	TERMINATE 		;Delete rejected.
";
            Assert.Pass(this.RunModel(input).ToString());
        }

        private IASTNode RunModel(string input)
        {
            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            ASTModel model = parser.Parse();
            return model;
        }
    }
}
