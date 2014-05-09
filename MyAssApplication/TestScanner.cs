using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler;

namespace MyAssApplication
{
    public static class TestScanner
    {
        public static void Run()
        {
            string source = TestSources.Source1();


            Scanner sc = new Scanner(new StringCharSource(source));

            while (sc.CurrentToken != TokenType.EOF)
            {
                sc.Next();
            }
        }
    }
}
