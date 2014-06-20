using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework.Entities
{
    public class FacilityEntity : AbstractEntity
    {
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
            this.contentTimeArea += (this.busy ? 1 : 0) * (Simulation.It.Clock - this.latestChangeClock);
            this.utilization = this.contentTimeArea / Simulation.It.Clock;
            this.averageTime = this.contentTimeArea / this.captureCount;

            this.latestChangeClock = Simulation.It.Clock;
        }
    }
}
