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
    [Category("ParserTests_v2_IdConflsicts")]
    public class ParserTests_IdConflsicts
    {
        [Test]
        public void DirectSna()
        {
            string input = @"Generate x$SomeId";
            Assert.Pass(this.Run(input).ToString());
        }


        [Test]
        public void Label_Block_Literal()
        {
            string input = @"Server STORAGE 3";
            Assert.Pass(this.Run(input).ToString());
        }

        [Test]
        public void Label_Block_Id()
        {
            string input = @"Server STORAGE X";
            Assert.Pass(this.Run(input).ToString());
        }

        [Test]
        public void Block_Literal()
        {
            string input = @"TERMINATE 1";
            Assert.Pass(this.Run(input).ToString());
        }

        [Test]
        public void Block_Literal2()
        {
            string input = @"TERMINATE 1 2 3 4 5";
            Assert.Pass(this.Run(input).ToString());
        }

        [Test]
        public void Block_Literal3()
        {
            string input = @"TERMINATE 1,2,3,54";
            Assert.Pass(this.Run(input).ToString());
        }

        private ASTAnyNode Run(string input)
        {
            input = Defaults.DefUsing + input;

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            ASTModel model = parser.Model;
            return model;
        }
    }
}
