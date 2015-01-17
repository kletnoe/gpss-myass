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
    [Category("ParserTests_v2_Accessors")]
    public class ParserTests_Accessors
    {
        [Test]
        public void Expr_Call()
        {
            string input = @"Generate Exponential(1,2,b)";
            Assert.Pass(this.RunModel(input).ToString());
        }

        [Test]
        public void Expr_DirSna()
        {
            string input = @"Generate X$some2";
            Assert.Pass(this.RunModel(input).ToString());
        }

        [Test]
        public void Block_Call()
        {
            string input = @"Generate Exponential(1,2,b)";
            Assert.Pass(this.RunModel(input).ToString());
        }

        [Test]
        public void Block_DirSna()
        {
            string input = @"Generate X$some2";
            Assert.Pass(this.RunModel(input).ToString());
        }

        [Test]
        public void Block_Call2()
        {
            string input = @"Generate Exponential(1,2,b),1";
            Assert.Pass(this.RunModel(input).ToString());
        }

        [Test]
        public void Block_DirSna2()
        {
            string input = @"Generate X$some2,1";
            Assert.Pass(this.RunModel(input).ToString());
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
