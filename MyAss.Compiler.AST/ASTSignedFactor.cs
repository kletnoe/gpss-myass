using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Compiler.AST
{
    public class ASTSignedFactor : IASTNode
    {
        public AddOperatorType? Operator { get; set; }
        public IASTFactor Value { get; set; }

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
            return (this.Operator.HasValue ? this.Operator.ToString() : "")
                + this.Value;
        }
    }
}
