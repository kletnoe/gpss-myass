using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTDirectSNA : IASTAccessor
    {
        public int Id { get; set; }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return "$" + String.Format("id({0})", this.Id);
        }
    }
}
