using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler;
using MyAss.Compiler_v2.AST;
using NUnit.Framework;

namespace MyAss.Compiler_v2.Tests.ParserTests
{
    [TestFixture]
    [Category("ParserTests_v2_Directives")]
    public class ParserTests_Directives
    {
        [Test]
        public void Directive_UsingID()
        {
            string input = @"@using Test";
            Assert.Pass(this.RunModel(input).ToString());
        }

        [Test]
        public void Directive_UsingQualID()
        {
            string input = @"@using   Test.Test";
            Assert.Pass(this.RunModel(input).ToString());
        }

        [Test]
        public void Directive_UsingpID()
        {
            string input = @"@usingp Test";
            Assert.Pass(this.RunModel(input).ToString());
        }

        [Test]
        public void Directive_UsingpQualID()
        {
            string input = @"@usingp Test.Test";
            Assert.Pass(this.RunModel(input).ToString());
        }

        [Test]
        public void Directive_Usings()
        {
            string input = @"
@usingp Test.Test
@using Some.Test

@usingp Test.Some
";
            Assert.Pass(this.RunModel(input).ToString());
        }

        private ASTAnyNode RunModel(string input)
        {
            Parser_v2 parser = new Parser_v2(new Scanner(new StringCharSource(input)));
            ASTModel model = parser.Model;
            return model;
        }
    }
}
