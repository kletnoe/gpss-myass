using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler.AST;

namespace MyAssCompiler
{
    public class ASTResolver : IASTResolver
    {
        public ASTModel Resolve(ASTModel model)
        {
            return model;
        }
    }
}
