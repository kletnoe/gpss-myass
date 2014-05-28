using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler.AST;
using NUnit.Framework;

namespace MyAssCompiler.Tests.ParserTests
{
    [TestFixture]
    public class ExpressionTests
    {
        [Test]
        public void Expression_Literal()
        {
            string input = @"Some 3";
            Assert.Pass(this.Run(input).ToString());
        }

        [Test]
        public void Expression_SignedLiteral1()
        {
            string input = @"Some -3";
            Assert.Pass(this.Run(input).ToString());
        }

        [Test]
        public void Expression_SignedLiteral2()
        {
            string input = @"Some +3";
            Assert.Pass(this.Run(input).ToString());
        }

        [Test]
        public void Expression_Id()
        {
            string input = @"Some someId";
            Assert.Pass(this.Run(input).ToString());
        }

        [Test]
        public void Expression_ParExpression()
        {
            string input = @"Some (someId)";
            Assert.Pass(this.Run(input).ToString());
        }

        [Test]
        public void Expression_Complex1()
        {
            string input = @"Some (someId + 3)/25";
            Assert.Pass(this.Run(input).ToString());
        }

        [Test]
        public void Expression_Complex2()
        {
            string input = @"Some (someId + 3)/25 + (1 + (id2 # (2 / 7)))";
            Assert.Pass(this.Run(input).ToString());
        }

        private IASTNode Run(string input)
        {
            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            ASTModel model = parser.Parse();
            return model;
        }
    }
}
