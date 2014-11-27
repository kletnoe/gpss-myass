using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler_v2.AST
{
    [DataContract]
    [KnownType(typeof(ASTBinaryExpression))]
    [KnownType(typeof(ASTDirectSNACall))]
    [KnownType(typeof(ASTProcedureCall))]
    [KnownType(typeof(ASTLValue))]
    [KnownType(typeof(ASTLiteral))]
    public class ASTBinaryExpression : ASTAnyExpression
    {
        [DataMember(Order = 0)]
        public ASTAnyExpression Left { get; set; }

        [DataMember(Order = 1)]
        public BinaryOperatorType Operator { get; set; }

        [DataMember(Order = 2)]
        public ASTAnyExpression Right { get; set; }

        public override T Accept<T>(IASTVisitor<T> visitor)
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
