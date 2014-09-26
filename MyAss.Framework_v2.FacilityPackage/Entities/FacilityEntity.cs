using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2.Entities;

namespace MyAss.Framework_v2.FacilityPackage.Entities
{
    public class FacilityEntity : AbstractEntity, IPendableEntity, IInterruptableEntity, IDelayableEntity
    {
        private Simulation simulation;

        public LinkedList<Transaction> PendingChain { get; private set; }
        public LinkedList<Transaction> InterruptChain { get; private set; }
        public LinkedList<Transaction> DelayChain { get; private set; }

        public bool IsAvaliable { get; private set; }
        public bool IsBusy { get; private set; }
        public int CaptureCount { get; private set; }
        public bool IsInterrupted { get; private set; }
        public double Utilization { get; private set; }
        public double AverageHoldingTime { get; private set; }
        public int OwnerId { get; private set; }

        public double LatestChangeClock { get; private set; }
        public double ContentTimeArea { get; private set; }

        public FacilityEntity()
        {
            OwnerId = 0;

            IsBusy = false;
            IsAvaliable = true;
            CaptureCount = 0;
            IsInterrupted = false;
            Utilization = 0.0;
            AverageHoldingTime = 0.0;

            LatestChangeClock = 0.0;
            ContentTimeArea = 0.0;
        }

        public FacilityEntity(int id)
            : this()
        {
            this.Id = id;
        }

        public override void UpdateStats()
        {
            this.ContentTimeArea += (this.IsBusy ? 1 : 0) * (this.simulation.Clock - this.LatestChangeClock);
            this.Utilization = this.ContentTimeArea / this.simulation.Clock;
            this.AverageHoldingTime = this.ContentTimeArea / this.CaptureCount;

            this.LatestChangeClock = this.simulation.Clock;
        }


        public override string GetStandardReportHeader()
        {
            return String.Format("{0,-14} {1,6} {2,9} {3,9} {4,6} {5,6} {6,6} {7,6} {8,6} {9,6}",
                "FACILITY", "ENTRY", "UTIL.", "AVE.TIME", "AVAIL", "OWNER", "PEND", "INTER", "RETRY", "DELAY");
        }

        public override string GetStandardReportLine()
        {
            return String.Format("{0,-14} {1,6} {2,9:F3} {3,9:F3} {4,6} {5,6} {6,6} {7,6} {8,6} {9,6}",
                this.simulation.NamesDictionary.GetByFirst(this.Id),
                this.CaptureCount,
                this.Utilization,
                this.AverageHoldingTime,
                this.IsAvaliable,
                this.OwnerId,
                this.PendingChain.Count,
                this.InterruptChain.Count,
                this.RetryChain.Count,
                this.DelayChain.Count);
        }

        #region SNA

        public double F { get { return this.IsBusy ? 1 : 0; } }
        public double FC { get { return this.CaptureCount; } }
        public double FI { get { return this.IsInterrupted ? 1 : 0; } }
        public double FR { get { return Math.Floor(this.Utilization * 1000); } }
        public double FT { get { return this.AverageHoldingTime; } }
        public double FV { get { return this.IsAvaliable ? 1 : 0; } }

        #endregion
    }
}
