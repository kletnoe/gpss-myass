using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MyAssCompiler.Tests
{
    [TestFixture]
    public class PrintVisitorTests
    {
        [Test]
        public void Test1()
        {
            string source = @"Server STORAGE 3";
            Assert.Pass(new ASTPrintVisitor(new Parser(new Scanner(new StringCharSource(source)))).Print());
        }

        [Test]
        public void Test2()
        {
            string source = @"SAVEVALUE GenerateCounter,1";
            Assert.Pass(new ASTPrintVisitor(new Parser(new Scanner(new StringCharSource(source)))).Print());
        }
    }
}
