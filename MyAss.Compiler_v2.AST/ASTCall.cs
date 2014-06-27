using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler_v2.AST
{
    public class ASTCall : IASTAccessor
    {
        public IList<IASTExpression> Actuals { get; private set; }

        public ASTCall()
        {
            this.Actuals = new List<IASTExpression>();
        }

        public T Accept<T>(IASTVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return "(" + String.Join(", ", this.Actuals) + ")";
        }
    }
}
