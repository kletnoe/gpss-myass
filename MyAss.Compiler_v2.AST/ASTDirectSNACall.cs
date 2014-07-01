using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler_v2.AST
{
    public class ASTDirectSNACall : IASTCall
    {
        public string SnaId { get; set; }
        public string ActualId { get; set; }

        public T Accept<T>(IASTVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return this.SnaId + "$" + this.ActualId;
        }
    }
}
