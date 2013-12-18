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
        private int currentCount;
        private bool avaliable;
        private int entriesCount;
        private int maxContent;
        private int minContent;

        private double utilization;

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
                throw new NotImplementedException();
            }
        }

        // SR
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
                throw new NotImplementedException();
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
            this.currentCount = 0;
            this.avaliable = true;
            this.entriesCount = 0;
            this.maxContent = 0;
            this.minContent = 0;
            this.utilization = 0;

            this.DelayChain = new LinkedList<Transaction>();
        }

        public void Enter(int unitsCount)
        {
            this.currentCount += unitsCount;

            // Stats update:
            this.entriesCount++;

            if (this.maxContent < this.currentCount)
            {
                this.maxContent = this.currentCount;
            }
        }

        public void Leave(int unitsCount)
        {
            this.currentCount -= unitsCount;

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
    }
}
