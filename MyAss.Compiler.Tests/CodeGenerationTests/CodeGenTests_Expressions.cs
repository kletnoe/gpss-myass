using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Compiler.AST;
using MyAss.Compiler.CodeGeneration;
using NUnit.Framework;
using System.CodeDom;

namespace MyAss.Compiler.Tests.CodeGenerationTests
{
    [TestFixture]
    [Category("CodeGenTests_Expressions")]
    public class CodeGenTests_Expressions
    {
        [Test]
        public void IntLiteral()
        {
            string input = @"Mark 1";
            string expected = @"1";

            CommonCode(input, expected);
        }

        [Test]
        public void SignedIntLiteralPlus()
        {
            string input = @"Mark +11";
            string expected = @"(0+11)";

            CommonCode(input, expected);
        }

        [Test]
        public void SignedIntLiteralMinus()
        {
            string input = @"Mark -12";
            string expected = @"(0-12)";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void SignedIntLiteral2()
        {
            string input = @"Mark -(+(-(-1)))";
            string expected = @"(0-(0+(0-(0-1))))";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void MulopIntLiteral()
        {
            string input = @"Mark 11#5/3";
            string expected = @"((11#5)/3)";

            CommonCode(input, expected);
        }

        [Test]
        public void MulopIntLiteral2()
        {
            string input = @"Mark 11#5\3";
            string expected = @"((11#5)\3)";

            CommonCode(input, expected);
        }

        [Test]
        public void MulopIntLiteralParents()
        {
            string input = @"Mark 11#(5/3)";
            string expected = @"(11#(5/3))";

            CommonCode(input, expected);
        }

        [Test]
        public void MulopIntLiteralNegative()
        {
            string input = @"Mark -2#25/4#3";
            string expected = @"((((0-2)#25)/4)#3)";

            CommonCode(input, expected);
        }


        [Test]
        public void AddopIntLiteral()
        {
            string input = @"Mark 1+2";
            string expected = @"(1+2)";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void AddopIntLiteral2()
        {
            string input = @"Mark 1+2-3";
            string expected = @"((1+2)-3)";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void AddopIntLiteralParens()
        {
            string input = @"Mark 1+(2+3)";
            string expected = @"(1+(2+3))";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void AddopIntLiteralExtraParens()
        {
            string input = @"Mark ((1+((2+3))))";
            string expected = @"(1+(2+3))";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void AddopIntLiteralNegative()
        {
            string input = @"Mark -1+2+3";
            string expected = @"(((0-1)+2)+3)";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void MixedIntLiteral()
        {
            string input = @"Mark ((4+5)/(2#3+1))";
            string expected = @"((4+5)/((2#3)+1))";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void DoubleLiteral()
        {
            string input = @"Mark 1.3+2.4";
            string expected = @"(1.3+2.4)";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void SpacesInExpressions()
        {
            string input = @"Mark (1 + 2)";
            string expected = @"(1+2)";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void SpacesInExpressions2()
        {
            string input = @"Mark (1 + (1 / 3))";
            string expected = @"(1+(1/3))";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void SpacesInExpressions3()
        {
            string input = @"Mark ((1 + 2) / 3)";
            string expected = @"((1+2)/3)";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void SpacesInExpressions4()
        {
            string input = @"Mark ((1 + 2) / ( 1-  (3 # 4)))";
            string expected = @"((1+2)/(1-(3#4)))";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void SpacesInExpressions5()
        {
            string input = @"Mark (-1 - 2) -3";
            string expected = @"((0-1)-2)";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        [Ignore]
        public void DirectSna()
        {
            string input = @"Mark X$Tail";
            string expected = @"(1.3+2.4)";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        public static void CommonCode(string input, string expected)
        {
            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            vis.VisitAll();
            CodeCompileUnit assembly = vis.CreateAssembly();
            string result = CodeDomHelper.Print(GetResultExpression(assembly));

            Assert.AreEqual(expected, result);
            Assert.Pass(result);
        }

        public static CodeExpression GetResultExpression(CodeCompileUnit assembly)
        {
            var result = ((assembly.Namespaces[0].Types[0].Members[1] as CodeMemberMethod)
                .Statements[1] as CodeAssignStatement).Right;
            return result;
        }
    }
}
