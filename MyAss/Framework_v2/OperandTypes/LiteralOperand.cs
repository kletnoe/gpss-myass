using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework_v2.OperandTypes
{
    public class LiteralOperand
    {
        public string Value { get; private set; }

        public LiteralOperand(string value)
        {
            this.Value = value;
        }
    }
}
