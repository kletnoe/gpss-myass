using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler_v2.AST
{
    public class ASTBinaryExpression : IASTExpression
    {
        public IASTExpression Left { get; set; }
        public BinaryOperatorType Operator { get; set; }
        public IASTExpression Right { get; set; }

        public T Accept<T>(IASTVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "("
                + this.Left.ToString()
                + " " + this.BinaryOperatorToString(this.Operator) + " "
                + this.Right.ToString()
                + ")";

        }

        private string BinaryOperatorToString(BinaryOperatorType op)
        {
            switch (op)
            {
                case BinaryOperatorType.Add:        return "+";
                case BinaryOperatorType.Substract:  return "-";
                case BinaryOperatorType.Multiply:   return "#";
                case BinaryOperatorType.Divide:     return "/";
                case BinaryOperatorType.Modulus:    return "\\";
                case BinaryOperatorType.Exponent:   return "^";
                default: return "";
            }
        }
    }
}
