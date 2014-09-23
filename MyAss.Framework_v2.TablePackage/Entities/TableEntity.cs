using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2.Entities;
using MyAss.Framework_v2.OperandTypes;

namespace MyAss.Framework_v2.TablePackage.Entities
{
    public class TableEntity : AbstractEntity
    {
        private Simulation simulation;

        IDoubleOperand tableArgument;
        double firstIntervalEndPoint;
        double intervalWidth;
        int intervalsCount;

        private int entriesCount = 0;
        private double mean = 0.0;
        private double standardDeviation = 0.0;

        private TableIntervals intervals;

        public TableEntity(Simulation simulation, int id, IDoubleOperand tableArgument,
            double firstIntervalEndPoint, double intervalWidth, int intervalsCount)
        {
            this.simulation = simulation;
            this.Id = id;

            this.tableArgument = tableArgument;
            this.firstIntervalEndPoint = firstIntervalEndPoint;
            this.intervalWidth = intervalWidth;
            this.intervalsCount = intervalsCount;

            this.intervals = new TableIntervals(firstIntervalEndPoint, intervalWidth, intervalsCount);
        }

        public double TB()
        {
            return this.mean;
        }

        public double TC()
        {
            return this.entriesCount;
        }

        public double TD()
        {
            return this.standardDeviation;
        }

        public void Tabulate(int weightedFactor)
        {
            this.entriesCount++;

            int value = (int)this.tableArgument.GetValue();

            this.IncrementInterval(value, weightedFactor);
        }

        private void IncrementInterval(double value, int weightedFactor)
        {
            TableInterval interval = this.intervals.Intervals.Where(x => x.IntervalStart < value && x.IntervalEnd >= value).First();
            interval.ObservedFrequency += weightedFactor;
        }

        public override void UpdateStats()
        {
            //throw new NotImplementedException();
        }

        public override string GetStandardReportHeader()
        {
            return "temp: TheTable!";
        }

        public override string GetStandardReportLine()
        {
            return this.GetIntervalsInfo();
        }

        private string GetIntervalsInfo()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var interval in this.intervals.Intervals)
            {
                sb.AppendLine(interval.ObservedFrequency + "\t" +interval.IntervalStart + " - " + interval.IntervalEnd);
            }

            return sb.ToString();
        }
    }
}
