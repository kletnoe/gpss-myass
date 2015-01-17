using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Compiler;
using MyAss.Compiler_v2;
using MyAss.Compiler_v2.AST;
using NUnit.Framework;

namespace MyAss.Compiler_v2.Tests.ParserTests
{
    [TestFixture]
    [Category("ParserTests_v2_Literals")]
    public class ParserTests_Literals
    {
        [Test]
        public void Literal_Int()
        {
            string input = @"TERMINATE 3";
            RunModel(input);
            //var result = parser.ExpectLiteral();

            //Assert.IsTrue(result.LiteralType == LiteralType.Int32, "result.LiteralType == AST.LiteralType.Int32");
            //Assert.IsInstanceOf<Int32>(result.Value, "IsInstanceOf<Int32>");
            //Assert.IsTrue((Int32)result.Value == 3, "(Int32)result.Value == 3");
            //Assert.Pass(result.ToString());
        }

        [Test]
        public void Literal_Double()
        {
            string input = @"TERMINATE 3.33";
            RunModel(input);
            //var result = parser.ExpectLiteral();

            //Assert.IsTrue(result.LiteralType == LiteralType.Double, "result.LiteralType == AST.LiteralType.Double");
            //Assert.IsInstanceOf<Double>(result.Value, "IsInstanceOf<Double>");
            //Assert.IsTrue((Double)result.Value == 3.33, "(Double)result.Value == 3.33");
            //Assert.Pass(result.ToString());
        }

        private ASTAnyNode RunModel(string input)
        {
            input = Defaults.DefUsing + input;

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            ASTModel model = parser.Model;
            return model;
        }
    }
}
