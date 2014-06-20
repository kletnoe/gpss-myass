using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.OperandTypes_Test
{
    public interface IOperand<T>
    {
        T GetValue();
    }
}
