using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework.OperandTypes_Test
{
    class StringLiteralOperand : IOperand<String>
    {
        public String GetValue()
        {
            throw new NotImplementedException();
        }
    }
}
