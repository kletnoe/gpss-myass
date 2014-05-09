using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTCommand : ASTVerb
    {
        public override void Accept(IASTVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
