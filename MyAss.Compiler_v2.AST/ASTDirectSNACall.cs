using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler_v2.AST
{
    [DataContract]
    public class ASTDirectSNACall : ASTAnyCall
    {
        [DataMember]
        public string SnaId { get; set; }

        [DataMember]
        public string ActualId { get; set; }

        public override T Accept<T>(IASTVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return this.SnaId + "$" + this.ActualId;
        }
    }
}
