using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MyAss.Compiler.Tests.ParserTests
{
    [TestFixture]
    [Category("ParserTests_Literals")]
    public class ParserTests_Literals
    {
        [Test]
        public void Literal_Int()
        {
            string input = @"3";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            var result = parser.ExpectLiteral();

            Assert.IsTrue(result.LiteralType == AST.LiteralType.Int32, "result.LiteralType == AST.LiteralType.Int32");
            Assert.IsInstanceOf<Int32>(result.Value, "IsInstanceOf<Int32>");
            Assert.IsTrue((Int32)result.Value == 3, "(Int32)result.Value == 3");
            Assert.Pass(result.ToString());
        }

        [Test]
        public void Literal_Double()
        {
            string input = @"3.33";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            var result = parser.ExpectLiteral();

            Assert.IsTrue(result.LiteralType == AST.LiteralType.Double, "result.LiteralType == AST.LiteralType.Double");
            Assert.IsInstanceOf<Double>(result.Value, "IsInstanceOf<Double>");
            Assert.IsTrue((Double)result.Value == 3.33, "(Double)result.Value == 3.33");
            Assert.Pass(result.ToString());
        }
    }
}
