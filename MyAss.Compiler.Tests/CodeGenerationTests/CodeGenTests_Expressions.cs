using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Compiler.AST;
using MyAss.Compiler.CodeGeneration;
using NUnit.Framework;
using System.CodeDom;
using MyAss.Compiler;

namespace MyAss.Compiler.Tests.CodeGenerationTests
{
    [TestFixture]
    [Category("CodeGenTests_Expressions")]
    public class CodeGenTests_Expressions
    {
        [Test]
        [Ignore("CommonCode.GetResultExpression fails")]
        public void IntLiteral()
        {
            string input = @"Generate 1";
            string expected = @"1";

            CommonCode(input, expected);
        }

        [Test]
        public void SignedIntLiteralPlus()
        {
            string input = @"Generate +11";
            string expected = @"(0+11)";

            CommonCode(input, expected);
        }

        [Test]
        public void SignedIntLiteralMinus()
        {
            string input = @"Generate -12";
            string expected = @"(0-12)";

            CommonCode(input, expected);
        }

        [Test]
        public void SignedIntLiteral2()
        {
            string input = @"Generate -(+(-(-1)))";
            string expected = @"(0-(0+(0-(0-1))))";

            CommonCode(input, expected);
        }

        [Test]
        public void MulopIntLiteral()
        {
            string input = @"Generate 11#5/3";
            string expected = @"((11#5)/3)";

            CommonCode(input, expected);
        }

        [Test]
        public void MulopIntLiteral2()
        {
            string input = @"Generate 11#5\3";
            string expected = @"((11#5)\3)";

            CommonCode(input, expected);
        }

        [Test]
        public void MulopIntLiteralParents()
        {
            string input = @"Generate 11#(5/3)";
            string expected = @"(11#(5/3))";

            CommonCode(input, expected);
        }

        [Test]
        public void MulopIntLiteralNegative()
        {
            string input = @"Generate -2#25/4#3";
            string expected = @"((((0-2)#25)/4)#3)";

            CommonCode(input, expected);
        }


        [Test]
        public void AddopIntLiteral()
        {
            string input = @"Generate 1+2";
            string expected = @"(1+2)";

            CommonCode(input, expected);
        }

        [Test]
        public void AddopIntLiteral2()
        {
            string input = @"Generate 1+2-3";
            string expected = @"((1+2)-3)";

            CommonCode(input, expected);
        }

        [Test]
        public void AddopIntLiteralParens()
        {
            string input = @"Generate 1+(2+3)";
            string expected = @"(1+(2+3))";

            CommonCode(input, expected);
        }

        [Test]
        public void AddopIntLiteralExtraParens()
        {
            string input = @"Generate ((1+((2+3))))";
            string expected = @"(1+(2+3))";

            CommonCode(input, expected);
        }

        [Test]
        public void AddopIntLiteralNegative()
        {
            string input = @"Generate -1+2+3";
            string expected = @"(((0-1)+2)+3)";

            CommonCode(input, expected);
        }

        [Test]
        public void MixedIntLiteral()
        {
            string input = @"Generate ((4+5)/(2#3+1))";
            string expected = @"((4+5)/((2#3)+1))";

            CommonCode(input, expected);
        }

        [Test]
        public void DoubleLiteral()
        {
            string input = @"Generate 1.3+2.4";
            string expected = @"(1.3+2.4)";

            CommonCode(input, expected);
        }

        [Test]
        public void SpacesInExpressions()
        {
            string input = @"Generate (1 + 2)";
            string expected = @"(1+2)";

            CommonCode(input, expected);
        }

        [Test]
        public void SpacesInExpressions2()
        {
            string input = @"Generate (1 + (1 / 3))";
            string expected = @"(1+(1/3))";

            CommonCode(input, expected);
        }

        [Test]
        public void SpacesInExpressions3()
        {
            string input = @"Generate ((1 + 2) / 3)";
            string expected = @"((1+2)/3)";

            CommonCode(input, expected);
        }

        [Test]
        public void SpacesInExpressions4()
        {
            string input = @"Generate ((1 + 2) / ( 1-  (3 # 4)))";
            string expected = @"((1+2)/(1-(3#4)))";

            CommonCode(input, expected);
        }

        [Test]
        public void SpacesInExpressions5()
        {
            string input = @"Generate (-1 - 2) -3";
            string expected = @"((0-1)-2)";

            CommonCode(input, expected);
        }

        [Test]
        public void DirectSna()
        {
            string input = @"Generate X$Tail";
            string expected = @"X(simulation, Tail)";

            CommonCode(input, expected);
        }

        [Test]
        public void Procedure()
        {
            string input = @"Generate Exponential(1,2,3)";
            string expected = @"Exponential(1, 2, 3)";

            CommonCode(input, expected);
        }

        public static void CommonCode(string input, string expected)
        {
            input = Defaults.DefUsing + input;

            Parser parser = new Parser(new Scanner(new CharSourceTokenizer(input)));
            CodeDomGenerationVisitor vis = new CodeDomGenerationVisitor(parser.MetadataRetriever);

            CodeCompileUnit assembly = vis.VisitAll(parser.Model);
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
