using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework.Blocks;

namespace MyAssFramework.Entities
{
    public class StorageEntity : AbstractEntity
    {
        private int capacity;
        private int currentCount = 0;
        private bool avaliable = true;
        private int entriesCount = 0;
        private int maxContent = 0;
        private int minContent = 0;
        private double averageContent = 0.0;
        private double utilization = 0.0;
        private double averageTime = 0.0;

        private double latestChangeClock = 0.0;
        private double contentTimeArea = 0.0;

        public LinkedList<Transaction> DelayChain { get; set; }

        public int Capacity
        {
            get
            {
                return this.capacity;
            }
        }

        // S
        public int CurrentCount
        {
            get
            {
                return this.currentCount;
            }
        }

        // SC
        public int EntriesCount
        {
            get
            {
                return this.entriesCount;
            }
        }

        // SM
        public int MaxContent
        {
            get
            {
                return this.maxContent;
            }
        }

        public int MinContent
        {
            get
            {
                return this.minContent;
            }
        }

        // R
        public int RemainingCapacity
        {
            get
            {
                return this.capacity - this.currentCount;
            }
        }

        // SA
        public double AverageContent
        {
            get
            {
                return this.averageContent;
            }
        }

        // SR
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

        // ST
        public double AverageHoldingTime
        {
            get
            {
                return this.averageTime;
            }
        }

        // SE
        public bool IsEmpty
        {
            get
            {
                return this.currentCount == 0;
            }
        }

        // SF
        public bool IsFull
        {
            get
            {
                return this.currentCount == this.capacity;
            }
        }

        // SV
        public bool IsAvaliable
        {
            get
            {
                return this.avaliable;
            }
        }

        public StorageEntity(int id, int capacity)
        {
            this.Id = id;
            this.capacity = capacity;

            this.DelayChain = new LinkedList<Transaction>();
        }

        public void Enter(int units)
        {
            this.entriesCount += units;

            if (this.maxContent < this.currentCount)
            {
                this.maxContent = this.currentCount;
            }

            this.UpdateStats();

            this.currentCount += units;
        }

        public void Leave(int units)
        {
            this.UpdateStats();

            this.currentCount -= units;

            if (this.DelayChain.Count > 0)
            {
                Transaction transaction = this.DelayChain.First();
                this.DelayChain.RemoveFirst();
                transaction.NextEventTime = Simulation.It.Clock;
                ((Enter)Simulation.It.GetBlock(transaction.NextOwner)).PreEnter(transaction); // anti "line-bucking"
            }
        }

        public void SAvail()
        {
            this.avaliable = true;
        }

        public void SUnavail()
        {
            this.avaliable = false;
        }

        public override void UpdateStats()
        {
            this.contentTimeArea += this.currentCount * (Simulation.It.Clock - this.latestChangeClock);
            this.averageContent = this.contentTimeArea / Simulation.It.Clock;
            this.utilization = this.contentTimeArea / (this.capacity * Simulation.It.Clock);
            this.averageTime = this.contentTimeArea / this.entriesCount;

            this.latestChangeClock = Simulation.It.Clock;
        }
    }
}
