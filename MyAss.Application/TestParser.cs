using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Compiler;
using MyAss.Compiler.AST;

namespace MyAss.Application
{
    public static class TestParser
    {
        public static void Run()
        {
            string source = TestSources.Source1();
            ICharSource charSource = new StringCharSource(source);
            IScanner scanner = new Scanner(charSource);
            IParser parser = new Parser(scanner);
            ASTPrintVisitor visitor = new ASTPrintVisitor(parser);
            Console.WriteLine(visitor.Print());
        }
    }
}
