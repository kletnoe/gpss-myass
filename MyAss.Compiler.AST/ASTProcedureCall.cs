using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler.AST
{
    [DataContract]
    public class ASTProcedureCall : ASTAnyCall
    {
        [DataMember(Order = 0)]
        public string ProcedureId { get; set; }

        [DataMember(Order = 1)]
        public IList<ASTAnyExpression> Actuals { get; private set; }

        public ASTProcedureCall()
        {
            this.Actuals = new List<ASTAnyExpression>();
        }

        public override T Accept<T>(IASTVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return this.ProcedureId + "(" + String.Join(", ", this.Actuals) + ")";
        }
    }
}
