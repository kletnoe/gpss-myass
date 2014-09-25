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

        //private double latestValue = Double.NaN;

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
            double value = this.tableArgument.GetValue();
            this.IncrementInterval(value, weightedFactor);

            // Stats
            this.mean = (this.mean * (this.entriesCount - 1) + value) / (double)this.entriesCount;
        }

        private void IncrementInterval(double value, int weightedFactor)
        {
            TableInterval interval = this.intervals.Intervals.Where(x => x.IntervalStart < value && x.IntervalEnd >= value).First();
            interval.ObservedFrequency += weightedFactor;
        }

        public override void UpdateStats()
        {
            // This is empty 'cos Table stats are not in connection with Simulation.Clock.
        }

        public override string GetStandardReportHeader()
        {
            return String.Format("{0,-14} {1,8} {2,8} {3,6} {4,8} {5,5} {6,7} {7,9} {8,6}",
                "TABLE", "MEAN", "STD.DEV.", "RETRY", "", "RANGE", "", "FREQUENCY", "CUM.%");
        }

        public override string GetStandardReportLine()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(String.Format("{0,-14} {1,8:F3} {2,8:F3} {3,6}",
                this.simulation.NamesDictionary.GetByFirst(this.Id),
                this.mean,
                this.standardDeviation,
                this.RetryChain.Count));

            sb.AppendLine(this.GetIntervalsInfo());

            return sb.ToString();
        }

        private string GetIntervalsInfo()
        {
            StringBuilder sb = new StringBuilder();

            int cumulativeSum = 0;

            foreach (var interval in this.intervals.Intervals)
            {
                cumulativeSum += interval.ObservedFrequency;
                double cumulativePercent = ((double)cumulativeSum / (double)this.entriesCount) * 100;

                sb.AppendLine(String.Format("{0,40} {1,9} {2,1} {3,9} {4,9} {5,6:F2}",
                    String.Empty,
                    Double.IsNegativeInfinity(interval.IntervalStart) ? "-Inf." : interval.IntervalStart.ToString("F3"),
                    "-",
                    Double.IsPositiveInfinity(interval.IntervalEnd) ? "Inf." : interval.IntervalEnd.ToString("F3"),
                    interval.ObservedFrequency,
                    cumulativePercent));
            }

            return sb.ToString();
        }
    }
}
