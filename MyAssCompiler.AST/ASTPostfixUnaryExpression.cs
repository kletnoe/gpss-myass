using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTPostfixUnaryExpression : ASTExpression
    {
        public ASTExpression Value { get; set; }
        public PostfixUnaryOperatorType Operator { get; set; }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            string op;

            switch (this.Operator)
            {
                case PostfixUnaryOperatorType.PLUS:
                    op = "+";
                    break;
                case PostfixUnaryOperatorType.MINUS:
                    op = "-";
                    break;
                default:
                    op = "!";
                    break;
            }

            return this.Value + op;
        }
    }
}
