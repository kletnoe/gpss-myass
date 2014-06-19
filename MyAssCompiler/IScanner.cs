using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler
{
    public interface IScanner
    {
        TokenType CurrentToken { get; }
        object CurrentTokenVal { get; }
        int CurrentTokenLine { get; }
        int CurrentTokenColumn { get; }
        IList<string> Identifiers { get; }
        bool IgnoreWhitespace { get; set; }

        void Next();
    }
}
