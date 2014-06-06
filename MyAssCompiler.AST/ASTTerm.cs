using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTTerm : IASTNode
    {
        public ASTSignedFactor Factor { get; set; }
        public IList<ASTMultiplicative> Multiplicatives { get; private set; }

        public ASTTerm()
        {
            this.Multiplicatives = new List<ASTMultiplicative>();
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return this.Factor.ToString()
                + (this.Multiplicatives.Count != 0 ? " " + String.Join(" ", this.Multiplicatives) : "");
        }
    }
}
