using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTBinaryExpression : ASTExpression
    {
        public ASTExpression LValue { get; set; }
        public ASTExpression RValue { get; set; }
        public BinaryOperatorType Operator { get; set; }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return this.LValue + " "
                + this.Operator.ToString()
                + " " + this.RValue;
        }
    }
}
