using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler.AST;

namespace MyAssCompiler
{
    public interface IASTResolver
    {
        ASTModel Resolve(ASTModel model);
    }
}
