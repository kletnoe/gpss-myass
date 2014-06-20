using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Compiler.AST;

namespace MyAss.Compiler
{
    public class ASTResolver : IASTResolver
    {
        public ASTModel Resolve(ASTModel model)
        {
            return model;
        }
    }
}
