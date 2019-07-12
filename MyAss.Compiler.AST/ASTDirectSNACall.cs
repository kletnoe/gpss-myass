using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler.AST
{
    [DataContract]
    public class ASTDirectSNACall : ASTAnyCall
    {
        [DataMember(Order = 0)]
        public string SnaId { get; set; }

        [DataMember(Order = 1)]
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
