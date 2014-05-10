using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler.AST;
using NUnit.Framework;

namespace MyAssCompiler.Tests
{
    [TestFixture]
    public class IdConflictTests
    {
        [Test]
        public void DirectSna()
        {
            string input = @"Init x$SomeId";

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

        //[Test]
        //public void T
    }
}
