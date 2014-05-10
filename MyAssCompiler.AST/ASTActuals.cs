using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTActuals : IASTNode
    {
        public IList<ASTExpression> Expressions { get; set; }

        public ASTActuals()
        {
            this.Expressions = new List<ASTExpression>();
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return String.Join(",", this.Expressions);
        }
    }
}
