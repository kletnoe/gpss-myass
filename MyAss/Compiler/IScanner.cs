using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Compiler
{
    public interface IScanner
    {
        TokenType CurrentToken { get; }
        object CurrentTokenVal { get; }
        int CurrentTokenLine { get; }
        int CurrentTokenColumn { get; }
        string CurrentLine { get; }

        bool IgnoreWhitespace { get; set; }

        void Next();
    }
}
