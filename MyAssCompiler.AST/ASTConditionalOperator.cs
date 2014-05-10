using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTConditionalOperator : ASTOperator
    {
        public ConditionalOperatorType Operator { get; set; }

        public override string ToString()
        {
            return this.Operator.ToString();
        }
    }
}
