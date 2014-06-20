using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTAdditive : IASTNode
    {
        public AddOperatorType Operator { get; set; }
        public ASTTerm Term { get; set; }

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
            return this.Operator.ToString() + " "
                + this.Term.ToString();
        }
    }
}
