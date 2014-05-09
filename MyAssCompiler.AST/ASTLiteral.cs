using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTLiteral : ASTExpression
    {
        public LiteralType LiteralType { get; set; }
        public object Value { get; set; }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
