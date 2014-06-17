using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTModel : IASTNode
    {
        public IList<ASTVerb> Verbs { get; private set; }

        public ASTModel()
        {
            this.Verbs = new List<ASTVerb>();
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
            return String.Join(Environment.NewLine, this.Verbs) + Environment.NewLine;
        }
    }
}
