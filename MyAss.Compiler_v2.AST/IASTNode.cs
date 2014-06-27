using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler_v2.AST
{
    public interface IASTNode
    {
        T Accept<T>(IASTVisitor<T> visitor);
    }
}
