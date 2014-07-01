using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler_v2.AST
{
    public class ASTProcedureCall : IASTCall
    {
        public string ProcedureId { get; set; }
        public IList<IASTExpression> Actuals { get; private set; }

        public ASTProcedureCall()
        {
            this.Actuals = new List<IASTExpression>();
        }

        public T Accept<T>(IASTVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return this.ProcedureId + "(" + String.Join(", ", this.Actuals) + ")";
        }
    }
}
