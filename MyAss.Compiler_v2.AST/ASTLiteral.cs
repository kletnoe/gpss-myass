using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler_v2.AST
{
    public class ASTLiteral : IASTExpression
    {
        public LiteralType LiteralType { get; set; }
        public object Value { get; set; }

        public T Accept<T>(IASTVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
