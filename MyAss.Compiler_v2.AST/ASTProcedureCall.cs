using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler_v2.AST
{
    [DataContract]
    public class ASTProcedureCall : ASTAnyCall
    {
        [DataMember]
        public string ProcedureId { get; set; }

        [DataMember]
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
