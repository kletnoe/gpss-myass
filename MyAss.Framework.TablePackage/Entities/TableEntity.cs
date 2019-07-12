using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework.Entities;
using MyAss.Framework.OperandTypes;

namespace MyAss.Framework.TablePackage.Entities
{
    public class TableEntity : AnyEntity
    {
        private Simulation simulation;

        public TableIntervals Intervals { get; private set; }
        public IDoubleOperand TableArgument { get; private set; }

        public int EntriesCount { get; private set; }
        public double Mean { get; private set; }
        public double AccumulatedSumOfSquares { get; private set; }
        public double SampleStandardDeviation { get; private set; }
        //public double PopulationStandardDeviation { get; private set; }

        public double FirstIntervalEndPoint { get; private set; }
        public double IntervalWidth { get; private set; }
        public int IntervalsCount { get; private set; }

        public TableEntity()
        {
            EntriesCount = 0;
            Mean = 0.0;
            AccumulatedSumOfSquares = 0.0;
            SampleStandardDeviation = 0.0;
            //PopulationStandardDeviation = 0.0;
        }

        public TableEntity(Simulation simulation, int id, IDoubleOperand tableArgument,
            double firstIntervalEndPoint, double intervalWidth, int intervalsCount)
            : this()
        {
            this.simulation = simulation;
            this.Id = id;

            this.TableArgument = tableArgument;
            this.FirstIntervalEndPoint = firstIntervalEndPoint;
            this.IntervalWidth = intervalWidth;
            this.IntervalsCount = intervalsCount;

            this.Intervals = new TableIntervals(firstIntervalEndPoint, intervalWidth, intervalsCount);
        }

        public void Tabulate(int weightedFactor)
        {
            this.EntriesCount++;
            double value = this.TableArgument.GetValue();
            this.IncrementInterval(value, weightedFactor);

            // Stats
            this.Mean = (this.Mean * (this.EntriesCount - 1) + value) / (double)this.EntriesCount;
            this.AccumulatedSumOfSquares += Math.Pow(value, 2);
            this.SampleStandardDeviation = Math.Sqrt(
                (this.AccumulatedSumOfSquares - (Math.Pow(this.Mean, 2) * this.EntriesCount)) 
                / (double)(this.EntriesCount - 1)
            );
        }

        private void IncrementInterval(double value, int weightedFactor)
        {
            TableInterval interval = this.Intervals.Intervals.Where(x => x.IntervalStart < value && x.IntervalEnd >= value).First();
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
                this.simulation.NamesAndVarsDictionary.GetNameByValue(this.Id),
                this.Mean,
                this.SampleStandardDeviation,
                this.RetryChain.Count));

            sb.AppendLine(this.GetIntervalsInfo());

            return sb.ToString();
        }

        private string GetIntervalsInfo()
        {
            StringBuilder sb = new StringBuilder();

            int cumulativeSum = 0;

            foreach (var interval in this.Intervals.Intervals)
            {
                cumulativeSum += interval.ObservedFrequency;
                double cumulativePercent = ((double)cumulativeSum / (double)this.EntriesCount) * 100;

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

        #region SNA

        public double TB { get { return this.Mean; } }
        public double TC { get { return this.EntriesCount; } }
        public double TD { get { return this.SampleStandardDeviation; } }

        #endregion
    }
}
