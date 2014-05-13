using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTExpression : ASTOperand, IASTFactor
    {
        public ASTTerm LTerm { get; set; }
        public AddOperatorType? Operator { get; set; }
        public ASTTerm RTerm { get; set; }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return  "("
                + this.LTerm.ToString()
                + (this.Operator.HasValue ? " " + this.Operator.ToString() : "")
                + (this.RTerm != null ? " " + this.RTerm.ToString() : "")
                + ")";
        }
    }
}
