using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler.AST;
using MyAssCompiler.CodeGeneration;
using NUnit.Framework;
using System.CodeDom;

namespace MyAssCompiler.Tests.CodeGenerationTests
{
    [TestFixture]
    public class CodeGenTests_Expressions
    {
        [Test]
        public void IntLiteral()
        {
            string input = @"Some 1";
            string expected = @"1";

            CommonCode(input, expected);
        }

        [Test]
        public void SignedIntLiteralPlus()
        {
            string input = @"Some +11";
            string expected = @"(0+11)";

            CommonCode(input, expected);
        }

        [Test]
        public void SignedIntLiteralMinus()
        {
            string input = @"Some -12";
            string expected = @"(0-12)";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void SignedIntLiteral2()
        {
            string input = @"Some -(+(-(-1)))";
            string expected = @"(0-(0+(0-(0-1))))";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void MulopIntLiteral()
        {
            string input = @"Some 11#5/3";
            string expected = @"((11#5)/3)";

            CommonCode(input, expected);
        }

        [Test]
        public void MulopIntLiteral2()
        {
            string input = @"Some 11#5\3";
            string expected = @"((11#5)\3)";

            CommonCode(input, expected);
        }

        [Test]
        public void MulopIntLiteralParents()
        {
            string input = @"Some 11#(5/3)";
            string expected = @"(11#(5/3))";

            CommonCode(input, expected);
        }

        [Test]
        public void MulopIntLiteralNegative()
        {
            string input = @"Some -2#25/4#3";
            string expected = @"((((0-2)#25)/4)#3)";

            CommonCode(input, expected);
        }


        [Test]
        public void AddopIntLiteral()
        {
            string input = @"Some 1+2";
            string expected = @"(1+2)";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void AddopIntLiteral2()
        {
            string input = @"Some 1+2-3";
            string expected = @"((1+2)-3)";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void AddopIntLiteralParens()
        {
            string input = @"Some 1+(2+3)";
            string expected = @"(1+(2+3))";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void AddopIntLiteralExtraParens()
        {
            string input = @"Some ((1+((2+3))))";
            string expected = @"(1+(2+3))";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void AddopIntLiteralNegative()
        {
            string input = @"Some -1+2+3";
            string expected = @"(((0-1)+2)+3)";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void MixedIntLiteral()
        {
            string input = @"Some (4+5)/(2#3+1)";
            string expected = @"((4+5)/((2#3)+1))";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void DoubleLiteral()
        {
            string input = @"Some 1.3+2.4";
            string expected = @"(1.3+2.4)";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);

            CommonCode(input, expected);
        }

        [Test]
        public void DirectSna()
        {
            string input = @"Some X$Tail";
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
