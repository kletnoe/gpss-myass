using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler_v2.AST
{
    public class ASTVerb : IASTNode
    {
        public string LabelId { get; set; }
        public string VerbId { get; set; }
        public IList<IASTExpression> Operands { get; private set; }

        public ASTVerb()
        {
            this.Operands = new List<IASTExpression>();
        }

        public T Accept<T>(IASTVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return (this.LabelId != null ? this.LabelId + " " : "")
                + this.VerbId + " "
                + String.Join(",", this.Operands);
        }
    }
}
