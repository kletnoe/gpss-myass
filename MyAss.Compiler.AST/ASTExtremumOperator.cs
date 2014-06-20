using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTExtremumOperator : ASTOperator
    {
        public ExtremumOperatorType Operator { get; set; }

        public override string ToString()
        {
            return this.Operator.ToString();
        }
    }
}
