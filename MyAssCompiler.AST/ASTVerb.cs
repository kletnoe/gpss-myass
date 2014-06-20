using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTVerb : IASTNode
    {
        public string LabelId { get; set; }
        public string VerbId { get; set; }
        public ASTOperands Operands { get; set; }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public T Accept<T>(IASTVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return (this.LabelId != null ? this.LabelId + " " : "")
                + this.VerbId + " "
                + String.Join(",", this.Operands);
        }
    }
}
