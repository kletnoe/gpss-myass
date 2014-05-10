using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTTerm : IASTNode
    {
        public ASTSignedFactor LValue { get; set; }
        public MulOperatorType? Operator { get; set; }
        public IASTFactor RValue { get; set; }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return this.LValue.ToString()
                + (this.Operator.HasValue ? " " + this.Operator.ToString() : "")
                + (this.RValue != null ? " " + this.RValue.ToString() : "");
        }
    }
}
