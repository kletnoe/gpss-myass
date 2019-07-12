using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler;
using MyAss.Compiler.AST;
using NUnit.Framework;

namespace MyAss.Compiler.Tests.ParserTests
{
    [TestFixture]
    [Category("ParserTests_Models")]
    public class ParserTests_Models
    {
        [Test]
        public void Model()
        {
            string input = TestModels.MM3Model_Simple;
            Assert.Pass(this.RunModel(input).ToString());
        }

        private ASTAnyNode RunModel(string input)
        {
            Parser parser = new Parser(new Scanner(new CharSourceTokenizer(input)));
            ASTModel model = parser.Model;
            return model;
        }
    }
}
