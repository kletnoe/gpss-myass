using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework_v2.OperandTypes
{
    public class NumberOperand : IDoubleOperand
    {
        public double value { get; private set; }

        public NumberOperand(double value)
        {
            this.value = value;
        }

        public double GetValue()
        {
            return this.value;
        }

        //public static implicit operator Number(double val)
        //{
        //    return new Number(val);
        //}
    }
}
