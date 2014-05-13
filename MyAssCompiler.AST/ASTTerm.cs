using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTTerm : IASTNode
    {
        public ASTSignedFactor LFactor { get; set; }
        public MulOperatorType? Operator { get; set; }
        public IASTFactor RFactor { get; set; }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return this.LFactor.ToString()
                + (this.Operator.HasValue ? " " + this.Operator.ToString() : "")
                + (this.RFactor != null ? " " + this.RFactor.ToString() : "");
        }
    }
}
