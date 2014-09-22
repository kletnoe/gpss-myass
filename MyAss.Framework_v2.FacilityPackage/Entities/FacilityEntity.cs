using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2.Entities;

namespace MyAss.Framework_v2.FacilityPackage.Entities
{
    public class FacilityEntity : AbstractEntity
    {
        private Simulation simulation;

        private int ownerId = 0;

        private bool busy = false;
        private bool avaliable = true;
        private int captureCount = 0;
        private bool interrupted = false;
        private double utilization = 0.0;
        private double averageTime = 0.0;

        private double latestChangeClock = 0.0;
        private double contentTimeArea = 0.0;

        public LinkedList<Transaction> PendingChain { get; set; }
        public LinkedList<Transaction> InterruptChain { get; set; }
        public LinkedList<Transaction> DelayChain { get; set; }

        //public Transaction Ownership { get; private set; }

        // FV
        public bool IsAvaliable
        {
            get
            {
                return this.avaliable;
            }
        }

        // F
        public bool IsBuisy
        {
            get
            {
                return this.busy;
            }
        }

        // FC
        public int CaptureCount
        {
            get
            {
                return this.captureCount;
            }
        }

        // FI
        public bool IsInterrupted
        {
            get
            {
                return this.interrupted;
            }
        }

        // FR
        public double FractionalUtilization
        {
            get
            {
                return Math.Floor(this.utilization * 1000);
            }
        }

        public double Utilization
        {
            get
            {
                return this.utilization;
            }
        }

        // FT
        public double AverageHoldingTime
        {
            get
            {
                return this.averageTime;
            }
        }

        public int Owner
        {
            get
            {
                return this.ownerId;
            }
        }

        public FacilityEntity(int id)
        {
            this.Id = id;
        }

        public override void UpdateStats()
        {
            this.contentTimeArea += (this.busy ? 1 : 0) * (this.simulation.Clock - this.latestChangeClock);
            this.utilization = this.contentTimeArea / this.simulation.Clock;
            this.averageTime = this.contentTimeArea / this.captureCount;

            this.latestChangeClock = this.simulation.Clock;
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
                this.Owner,
                this.PendingChain.Count,
                this.InterruptChain.Count,
                this.RetryChain.Count,
                this.DelayChain.Count);
        }
    }
}
