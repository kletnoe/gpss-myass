using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAssCompiler.CodeGeneration;
using NUnit.Framework;

namespace MyAssCompiler.Tests.CodeGenerationTests
{
    [TestFixture]
    class CodeGenTests_Verbs
    {
        [Test]
        [Ignore]
        public void DirectSna()
        {
            string input = @"Mark (12 + 13)";
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
