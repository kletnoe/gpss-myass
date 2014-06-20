using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler.AST;

namespace MyAssCompiler
{
    public interface IParser
    {
        //Dictionary<int,string> IdsList { get; }
        ASTModel Parse();
    }
}
