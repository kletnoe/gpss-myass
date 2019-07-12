using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework.TablePackage.Entities
{
    public class TableInterval
    {
        public double IntervalStart { get; private set; }
        public double IntervalEnd { get; private set; }
        public int ObservedFrequency { get; set; }

        public TableInterval(double intervalStart, double intervalEnd)
        {
            this.IntervalStart = intervalStart;
            this.IntervalEnd = intervalEnd;
            this.ObservedFrequency = 0;
        }
    }
}
