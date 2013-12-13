using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.RelationalOp
{
    public abstract class RelationalOperator
    {
        public static NE NE { get { return new NE(); } }
        public static EQ EQ { get { return new EQ(); } }
        public static LT LT { get { return new LT(); } }

        public abstract bool Compare(int lValue, int rValue);
        public abstract bool Compare(double lValue, double rValue);
    }
}
