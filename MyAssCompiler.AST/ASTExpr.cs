using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTExpr : ASTOperand, IASTFactor
    {
        public ASTTerm LValue { get; set; }
        public AddOperatorType? Operator { get; set; }
        public ASTTerm RValue { get; set; }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return  "("
                + this.LValue.ToString()
                + (this.Operator.HasValue ? " " + this.Operator.ToString() : "")
                + (this.RValue != null ? " " + this.RValue.ToString() : "")
                + ")";
        }
    }
}
