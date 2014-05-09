using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler;

namespace MyAssApplication
{
    public static class TestParser
    {
        public static void Run()
        {
            string source = TestSources.Source1();
            Console.WriteLine(new ASTPrintVisitor(new Parser(new Scanner(new StringCharSource(source)))).Print());
        }
    }
}
