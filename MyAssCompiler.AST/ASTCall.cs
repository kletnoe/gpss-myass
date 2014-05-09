using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTCall : ASTAccessor
    {
        public ASTActuals Actuals { get; set; }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return "(" + this.Actuals + ")";
        }
    }
}
