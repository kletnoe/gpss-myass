using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Compiler.AST
{
    public class ASTActuals : IASTNode
    {
        public IList<ASTExpression> Expressions { get; private set; }

        public ASTActuals()
        {
            this.Expressions = new List<ASTExpression>();
        }

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
            return String.Join(",", this.Expressions);
        }
    }
}
