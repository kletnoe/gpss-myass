using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTMultiplicative : IASTNode
    {
        public MulOperatorType Operator { get; set; }
        public IASTFactor Factor { get; set; }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return this.Operator.ToString() + " "
                + this.Factor.ToString();
        }
    }
}
