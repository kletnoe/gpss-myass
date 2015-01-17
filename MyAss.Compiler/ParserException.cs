using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler_v2;

namespace MyAss.Compiler
{
    public class ParserException : ApplicationException
    {
        public ParserException(Parser parser, string whatExpected)
            :base(String.Format("Expected {0} but got {1} at line {2} column {3}. Source line: {4}{5}",
                whatExpected,
                parser.Scanner.CurrentToken,
                parser.Scanner.CurrentTokenLine,
                parser.Scanner.CurrentTokenColumn,
                Environment.NewLine,
                parser.Scanner.CurrentLine.Insert(parser.Scanner.CurrentTokenColumn-1, ">")))
        {

        }
    }
}
