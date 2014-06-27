using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Compiler.AST
{
    public class ASTMultiplicative : IASTNode
    {
        public MulOperatorType Operator { get; set; }
        public IASTFactor Factor { get; set; }

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
                + (this.Factor is ASTExpression ? "(" + this.Factor.ToString() + ")" : this.Factor.ToString());
        }
    }
}
