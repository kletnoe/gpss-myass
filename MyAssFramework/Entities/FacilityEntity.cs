using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.Entities
{
    public class FacilityEntity : AbstractEntity
    {
        private bool busy;
        private bool avaliable;
        private int captureCount;
        private bool interrupted;

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
        public int Utilization
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        // FT
        public double AverageHoldingTime
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public FacilityEntity(int id)
        {
            this.Id = id;
            this.busy = false;
            this.avaliable = true;
            this.captureCount = 0;
            this.interrupted = false;
        }
    }
}
