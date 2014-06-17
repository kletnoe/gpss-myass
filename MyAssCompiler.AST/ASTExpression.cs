using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTExpression : ASTOperand, IASTFactor
    {
        public ASTTerm Term { get; set; }
        public IList<ASTAdditive> Additives { get; private set; }

        public ASTExpression()
        {
            this.Additives = new List<ASTAdditive>();
        }

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
            return "("
                + this.Term.ToString()
                + (this.Additives.Count != 0 ? " " + String.Join(" ", this.Additives) : "")
                + ")";
        }
    }
}
