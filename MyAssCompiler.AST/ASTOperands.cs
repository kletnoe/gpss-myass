using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTOperands : IASTNode
    {
        public IList<ASTOperand> Operands { get; set; }

        public ASTOperands()
        {
            this.Operands = new List<ASTOperand>();
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return String.Join(",", this.Operands);
        }
    }
}
