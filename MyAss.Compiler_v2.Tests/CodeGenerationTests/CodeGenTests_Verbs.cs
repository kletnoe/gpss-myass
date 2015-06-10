using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler;
using MyAss.Compiler_v2.CodeGeneration;
using NUnit.Framework;

namespace MyAss.Compiler_v2.Tests.CodeGenerationTests
{
    [TestFixture]
    [Category("CodeGenTests_v2_Verbs")]
    class CodeGenTests_Verbs
    {
        [Test]
        [Ignore]
        public void DirectSna()
        {
            string input = @"Mark X$ololo";
            string expected = @"(1.3+2.4)";

            CommonCode(input, expected);
        }

        public static void CommonCode(string input, string expected)
        {
            Parser parser = new Parser(new Scanner(new CharSourceTokenizer(input)));
            CodeDomGenerationVisitor vis = new CodeDomGenerationVisitor(parser.MetadataRetriever);

            CodeCompileUnit assembly = vis.VisitAll(parser.Model);
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
