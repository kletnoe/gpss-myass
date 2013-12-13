using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework.Blocks;

namespace MyAssFramework.Entities
{
    public class StorageEntity : Entity
    {
        private int capacity;
        private int currentCount;
        private bool avaliable;
        private int entriesCount;
        private int maxContent;

        public LinkedList<Transaction> DelayChain { get; set; }
        public LinkedList<Transaction> RetryChain { get; set; }

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
                throw new NotImplementedException();
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

            this.DelayChain = new LinkedList<Transaction>();
        }

        public void Enter(int unitsCount)
        {
            this.currentCount += unitsCount;
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

                transaction.NextEventTime = Simulation.Clock;

                Enter owner = (Enter)transaction.Owner;
                owner.PassTransaction(transaction);
                Simulation.CurrentEventChain.AddAhead(transaction); // anti "line-bucking"
            }

            //int freeUnits = unitsCount;
            //int chainIndex = 0;
            //while (freeUnits >= 0 && chainIndex <= this.DelayChain.Count)
            //{
            //    Transaction transaction = this.DelayChain.ElementAt(chainIndex);
            //    this.DelayChain.Remove(transaction);
            //    Enter owner = (Enter)transaction.Owner;
            //    owner.NextSequentialBlock.PassTransaction(transaction);
            //    Simulation.CurrentEventChain.AddBehind(transaction);
            //}


            //for (int i = this.DelayChain.Count - 1; i >= 0; i--)
            //{
            //    if (this.RemainingCapacity >= 1)
            //    {
            //        Transaction transaction = this.DelayChain.ElementAt(i);
            //        this.DelayChain.Remove(transaction);

            //        Enter owner = (Enter)transaction.Owner;
            //        owner.NextSequentialBlock.PassTransaction(transaction);
            //        Simulation.CurrentEventChain.AddBehind(transaction);
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}

            // TODO: Implement query from DelayChain
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
