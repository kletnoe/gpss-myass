using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler.AST
{
    [DataContract]
    public class ASTModel : ASTAnyNode
    {
        [DataMember(Order = 0)]
        public IList<ASTVerb> Verbs { get; private set; }

        public ASTModel()
        {
            this.Verbs = new List<ASTVerb>();
        }

        public override T Accept<T>(IASTVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return String.Join(Environment.NewLine, this.Verbs) + Environment.NewLine;
        }
    }
}
