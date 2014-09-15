using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework_v2.OperandTypes
{
    public interface IOperand<T>
    {
        T GetValue();
    }
}
