using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler.AST;
using NUnit.Framework;

namespace MyAssCompiler.Tests.ParserTests
{
    [TestFixture]
    public class ExpressionTests_Old
    {
        //[Test]
        //public void Expression_Literal()
        //{
        //    string input = @"3";

        //    Parser parser = new Parser(new Scanner(new StringCharSource(input)));
        //    var result = parser.ExpectExpression();

        //    Assert.IsInstanceOf<ASTLiteral>(result, "IsInstanceOf<ASTLiteral>");
        //    Assert.IsTrue((result as ASTLiteral).LiteralType == LiteralType.Int32, "LiteralType.Int32");
        //    Assert.IsTrue((Int32)(result as ASTLiteral).Value == 3, "Value == 3");
        //    Assert.Pass(result.ToString());
        //}

        //[Test]
        //public void Expression_Id()
        //{
        //    string input = @"someId";

        //    Parser parser = new Parser(new Scanner(new StringCharSource(input)));
        //    var result = parser.ExpectExpression();

        //    Assert.IsInstanceOf<ASTLValue>(result);
        //    Assert.IsTrue(parser.IdsList.Contains(new KeyValuePair<int, string>((result as ASTLValue).Id, "someId")), "Id not found in idsList");
        //    Assert.IsNull((result as ASTLValue).Accessor);
        //    Assert.Pass(result.ToString());
        //}

        ////[Test]
        ////public void Expression_DirectSna()
        ////{
        ////    string input = @"someSna$someId";

        ////    Parser parser = new Parser(new Scanner(new StringCharSource(input)));
        ////    var result = parser.ExpectExpression();

        ////    Assert.IsInstanceOf<ASTLValue>(result);
        ////    Assert.IsTrue(parser.IdsList.Contains(new KeyValuePair<int, string>((result as ASTLValue).Id, "someSna")), "Id not found in idsList");
        ////    Assert.IsInstanceOf<ASTDirectSNA>((result as ASTLValue).Accessor);

        ////    ASTDirectSNA sna = (result as ASTLValue).Accessor as ASTDirectSNA;
        ////    Assert.IsTrue(parser.IdsList.Contains(new KeyValuePair<int, string>(sna.Id, "someId")), "Id not found in idsList");

        ////    Assert.Pass(result.ToString());
        ////}

        //[Test]
        //public void Factor_Literal()
        //{
        //    string input = @"3";

        //    Parser parser = new Parser(new Scanner(new StringCharSource(input)));
        //    var result = parser.ExpectFactor();

        //    Assert.IsInstanceOf<ASTLiteral>(result, "IsInstanceOf<ASTLiteral>");
        //    Assert.IsTrue((result as ASTLiteral).LiteralType == LiteralType.Int32, "LiteralType.Int32");
        //    Assert.IsTrue((Int32)(result as ASTLiteral).Value == 3, "Value == 3");
        //    Assert.Pass(result.ToString());
        //}

        //[Test]
        //public void Factor_Id()
        //{
        //    string input = @"someId";

        //    Parser parser = new Parser(new Scanner(new StringCharSource(input)));
        //    var result = parser.ExpectFactor();

        //    Assert.IsInstanceOf<ASTLValue>(result);
        //    Assert.IsTrue(parser.IdsList.Contains(new KeyValuePair<int, string>((result as ASTLValue).Id, "someId")), "Id not found in idsList");
        //    Assert.IsNull((result as ASTLValue).Accessor);
        //    Assert.Pass(result.ToString());
        //}

        //[Test]
        //public void Factor_Expression()
        //{
        //    string input = @"(someId)";

        //    Parser parser = new Parser(new Scanner(new StringCharSource(input)));
        //    var result = parser.ExpectFactor();

        //    Assert.IsInstanceOf<ASTExpr>(result);
        //    Assert.Pass(result.ToString());
        //}
    }
}
