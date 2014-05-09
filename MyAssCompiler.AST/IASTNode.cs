using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public interface IASTNode
    {
        void Accept(IASTVisitor visitor);
    }
}
