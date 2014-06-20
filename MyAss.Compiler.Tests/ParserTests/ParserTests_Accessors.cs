using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Compiler.AST;
using NUnit.Framework;

namespace MyAss.Compiler.Tests.ParserTests
{
    [TestFixture]
    [Category("ParserTests_Accessors")]
    public class ParserTests_Accessors
    {
        [Test]
        public void Expr_Call()
        {
            string input = @"Some(1,2,b)";
            Assert.Pass(this.RunExpr(input).ToString());
        }

        [Test]
        public void Expr_DirSna()
        {
            string input = @"Some$some2";
            Assert.Pass(this.RunExpr(input).ToString());
        }

        private IASTNode RunExpr(string input)
        {
            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            return parser.ExpectExpression();
        }

        [Test]
        public void Block_Call()
        {
            string input = @"Mark Some(1,2,b)";
            Assert.Pass(this.RunModel(input).ToString());
        }

        [Test]
        public void Block_DirSna()
        {
            string input = @"Mark Some$some2";
            Assert.Pass(this.RunModel(input).ToString());
        }

        [Test]
        public void Block_Call2()
        {
            string input = @"Mark Some(1,2,b),1";
            Assert.Pass(this.RunModel(input).ToString());
        }

        [Test]
        public void Block_DirSna2()
        {
            string input = @"Mark Some$some2,1";
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
