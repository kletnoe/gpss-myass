using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Compiler.AST;
using NUnit.Framework;

namespace MyAss.Compiler.Tests
{
    [TestFixture]
    [Category("ParserTests_IdConflsicts")]
    public class ParserTests_IdConflsicts
    {
        [Test]
        public void DirectSna()
        {
            string input = @"Mark x$SomeId";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            var result = parser.Parse();
        }


        [Test]
        public void Label_Block_Literal()
        {
            string input = @"Server STORAGE 3";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            var model = parser.Parse();
            Assert.Pass(model.ToString());
        }

        [Test]
        public void Label_Block_Id()
        {
            string input = @"Server STORAGE X";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            var model = parser.Parse();
            Assert.Pass(model.ToString());
        }

        [Test]
        public void Block_Literal()
        {
            string input = @"TERMINATE 1";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            var model = parser.Parse();
            Assert.Pass(model.ToString());
        }

        [Test]
        public void Block_Literal2()
        {
            string input = @"TERMINATE 1 2 3 4 5";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            var model = parser.Parse();
            Assert.Pass(model.ToString());
        }

        [Test]
        public void Block_Literal3()
        {
            string input = @"TERMINATE 1,2,3,54";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            var model = parser.Parse();
            Assert.Pass(model.ToString());
        }
    }
}
