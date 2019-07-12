using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework.TablePackage.Entities
{
    public class TableIntervals
    {
        public List<TableInterval> Intervals { get; private set; }

        public TableIntervals(double firstIntervalEndPoint, double intervalWidth, int intervalsCount)
        {
            this.Intervals = new List<TableInterval>(intervalsCount);

            this.Intervals.Add(new TableInterval(Double.NegativeInfinity, firstIntervalEndPoint));

            // Iterate through second to last-1;
            for (int i = 1; i < intervalsCount - 1; i++)
            {
                double intervalStart = ((i - 1) * intervalWidth) + firstIntervalEndPoint;
                double intervalEnd = (i * intervalWidth) + firstIntervalEndPoint;
                this.Intervals.Add(new TableInterval(intervalStart, intervalEnd));
            }

            double lastIntervalStart = ((intervalsCount - 2) * intervalWidth) + firstIntervalEndPoint;
            this.Intervals.Add(new TableInterval(lastIntervalStart, Double.PositiveInfinity));
        }
    }
}
