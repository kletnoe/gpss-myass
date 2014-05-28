using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler.AST;
using MyAssCompiler.CodeGeneration;
using NUnit.Framework;

namespace MyAssCompiler.Tests.CodeGenerationTests
{
    [TestFixture]
    public class CodeGenTests
    {
        [Test]
        [Ignore]
        public void IntLiterals()
        {
            string input = @"Init 1,2,3";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);
            vis.Run();
        }

        [Test]
        [Ignore]
        public void SignedIntLiterals()
        {
            string input = @"Init -1,+2,-3";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);
            vis.Run();
        }

        [Test]
        [Ignore]
        public void SignedIntLiterals2()
        {
            string input = @"Init -(+(-(-1))),+2,3";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);
            vis.Run();
        }

        [Test]
        //[Ignore]
        public void AddopIntLiterals()
        {
            string input = @"Init 1+2";

            Parser parser = new Parser(new Scanner(new StringCharSource(input)));
            CodeGenerationVisitor vis = new CodeGenerationVisitor(parser);
            vis.Run();
        }
    }
}
