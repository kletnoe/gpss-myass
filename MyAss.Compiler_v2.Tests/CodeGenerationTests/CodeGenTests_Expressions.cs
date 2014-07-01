using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Compiler_v2.AST;
using MyAss.Compiler_v2.CodeGeneration;
using NUnit.Framework;
using System.CodeDom;
using MyAss.Compiler;

namespace MyAss.Compiler_v2.Tests.CodeGenerationTests
{
    [TestFixture]
    [Category("CodeGenTests_v2_Expressions")]
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

            CommonCode(input, expected);
        }

        [Test]
        public void SignedIntLiteral2()
        {
            string input = @"Mark -(+(-(-1)))";
            string expected = @"(0-(0+(0-(0-1))))";

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

            CommonCode(input, expected);
        }

        [Test]
        public void AddopIntLiteral2()
        {
            string input = @"Mark 1+2-3";
            string expected = @"((1+2)-3)";

            CommonCode(input, expected);
        }

        [Test]
        public void AddopIntLiteralParens()
        {
            string input = @"Mark 1+(2+3)";
            string expected = @"(1+(2+3))";

            CommonCode(input, expected);
        }

        [Test]
        public void AddopIntLiteralExtraParens()
        {
            string input = @"Mark ((1+((2+3))))";
            string expected = @"(1+(2+3))";

            CommonCode(input, expected);
        }

        [Test]
        public void AddopIntLiteralNegative()
        {
            string input = @"Mark -1+2+3";
            string expected = @"(((0-1)+2)+3)";

            CommonCode(input, expected);
        }

        [Test]
        public void MixedIntLiteral()
        {
            string input = @"Mark ((4+5)/(2#3+1))";
            string expected = @"((4+5)/((2#3)+1))";

            CommonCode(input, expected);
        }

        [Test]
        public void DoubleLiteral()
        {
            string input = @"Mark 1.3+2.4";
            string expected = @"(1.3+2.4)";

            CommonCode(input, expected);
        }

        [Test]
        public void SpacesInExpressions()
        {
            string input = @"Mark (1 + 2)";
            string expected = @"(1+2)";

            CommonCode(input, expected);
        }

        [Test]
        public void SpacesInExpressions2()
        {
            string input = @"Mark (1 + (1 / 3))";
            string expected = @"(1+(1/3))";

            CommonCode(input, expected);
        }

        [Test]
        public void SpacesInExpressions3()
        {
            string input = @"Mark ((1 + 2) / 3)";
            string expected = @"((1+2)/3)";

            CommonCode(input, expected);
        }

        [Test]
        public void SpacesInExpressions4()
        {
            string input = @"Mark ((1 + 2) / ( 1-  (3 # 4)))";
            string expected = @"((1+2)/(1-(3#4)))";

            CommonCode(input, expected);
        }

        [Test]
        public void SpacesInExpressions5()
        {
            string input = @"Mark (-1 - 2) -3";
            string expected = @"((0-1)-2)";

            CommonCode(input, expected);
        }

        [Test]
        public void DirectSna()
        {
            string input = @"Mark X$Tail";
            string expected = @"X(Tail)";

            CommonCode(input, expected);
        }

        [Test]
        public void Procedure()
        {
            string input = @"Mark Exponential(1,2,3)";
            string expected = @"Exponential(1, 2, 3)";

            CommonCode(input, expected);
        }

        public static void CommonCode(string input, string expected)
        {
            Parser_v2 parser = new Parser_v2(new Scanner(new StringCharSource(input)));
            CodeDomGenerationVisitor vis = new CodeDomGenerationVisitor(parser);

            CodeCompileUnit assembly = vis.VisitAll();
            string result = CodeDomHelper.Print(GetResultExpression(assembly));

            Assert.AreEqual(expected, result);
            Assert.Pass(result);
        }

        public static CodeExpression GetResultExpression(CodeCompileUnit assembly)
        {
            var members = assembly.Namespaces[0].Types[0].Members;
            CodeMemberMethod operandMethod = new CodeMemberMethod();

            for (int i = 0; i < members.Count; i++)
            {
                if (members[i].Name == "Verb1_Operand1")
                {
                    operandMethod = (CodeMemberMethod)members[i];
                }
            }

            var result = (operandMethod.Statements[1] as CodeAssignStatement).Right;
            return result;
        }
    }
}
