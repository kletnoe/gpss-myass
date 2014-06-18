using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.OperandTypes_Test
{
    class StringLiteralOperand : IOperand<String>
    {
        public String GetValue()
        {
            throw new NotImplementedException();
        }
    }
}
