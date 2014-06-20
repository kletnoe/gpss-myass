using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Compiler;

namespace MyAss.Application
{
    public static class Compiler
    {
        public static void RunDefaultCompiler()
        {
            string source = System.IO.File.ReadAllText(@".\Source.txt");

            IScanner s = new Scanner(new StringCharSource(source));

            while (s.CurrentToken != TokenType.EOF)
            {
                s.Next();
            }

            Console.WriteLine();
        }

        public static void TestCodeGen()
        {
            CodeGenerator cg = new CodeGenerator();
            cg.Some();
        }
    }
}
