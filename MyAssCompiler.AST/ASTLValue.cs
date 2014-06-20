using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTLValue : IASTFactor
    {
        public string Id { get; set; }
        public IASTAccessor Accessor { get; set; }

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
            return this.Id + this.Accessor;
        }
    }
}
