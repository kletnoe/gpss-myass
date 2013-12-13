using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.OperandTypes_Test
{
    public class ParExpression : Operand
    {
        public ExpressionDelegate value { get; private set; }

        public ParExpression(ExpressionDelegate expression)
        {
            this.value = expression;
        }

        public double GetValue()
        {
            return value.Invoke();
        }
    }
}
