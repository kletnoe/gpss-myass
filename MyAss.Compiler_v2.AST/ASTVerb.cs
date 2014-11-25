using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler_v2.AST
{
    [DataContract]
    [KnownType(typeof(ASTBinaryExpression))]
    [KnownType(typeof(ASTDirectSNACall))]
    [KnownType(typeof(ASTProcedureCall))]
    [KnownType(typeof(ASTLValue))]
    [KnownType(typeof(ASTLiteral))]
    public class ASTVerb : ASTAnyNode
    {
        [DataMember]
        public string LabelId { get; set; }

        [DataMember]
        public string VerbId { get; set; }

        [DataMember]
        public IList<ASTAnyExpression> Operands { get; private set; }

        public ASTVerb()
        {
            this.Operands = new List<ASTAnyExpression>();
        }

        public override T Accept<T>(IASTVisitor<T> visitor)
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
