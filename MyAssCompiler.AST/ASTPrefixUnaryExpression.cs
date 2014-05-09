using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTPrefixUnaryExpression : ASTExpression
    {
        public ASTExpression Value { get; set; }
        public PrefixUnaryOperatorType Operator { get; set; }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            string op;

            switch (this.Operator)
            {
                case PrefixUnaryOperatorType.PLUS:
                    op = "+";
                    break;
                case PrefixUnaryOperatorType.MINUS:
                    op = "-";
                    break;
                default:
                    op = "!";
                    break;
            }

            return op + this.Value;
        }
    }
}
