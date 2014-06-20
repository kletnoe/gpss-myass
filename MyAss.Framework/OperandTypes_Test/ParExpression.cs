using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework.OperandTypes_Test
{
    public class ParExpression : IDoubleOperand
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
