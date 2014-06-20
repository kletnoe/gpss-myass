using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using MyAss.Framework.Blocks;

namespace MyAss.Framework
{
    public class Transaction
    {
        public enum TransactionState
        {
            Active = 1,
            Suspended = 2,
            Passive = 3,
            Terminated = 4
        }

        public int Owner { get; set; }
        public int NextOwner { get; set; }

        public double NextEventTime { get; set; }

        /**/

        private TransactionParameters parameters;

        public TransactionParameters Parameters
        {
            get
            {
                if (this.parameters == null)
                {
                    this.parameters = new TransactionParameters();
                }

                return parameters;
            }
        }

        public int Priority { get; set; }

        public double MarkTime { get; set; }

        public int Number { get; private set; }

        public int AssemblySet { get; private set; }

        public bool DelayIndicator { get; private set; }

        public bool TraceIndicator { get; private set; }

        //public int CurrentBlock { get; private set; }

        //public int NextBlock { get; private set; }

        public TransactionState State { get; private set; }

        public bool IsPreempted { get; private set; }

        public Transaction()
        {
            int number = Simulation.It.NextTransactionNo();
            this.AssemblySet = number;
            this.Number = number;

            this.Priority = 0;
            this.MarkTime = Simulation.It.Clock;
            this.DelayIndicator = false;
            this.TraceIndicator = false;
            //this.CurrentBlock = 0;
            //this.NextBlock = 0;
            this.State = TransactionState.Active;
            this.IsPreempted = false;
        }

        public Transaction(int priority, int ownerId) : this()
        {
            this.Priority = priority;
            this.Owner = ownerId;
        }

        public override string ToString()
        {
            // return String.Format("Time: {0}, Prio: {1}, Clock: {2} ", CreationRealTime.ToString("HH:mm:ss.fff"), Priority, GenerationTime);
            return String.Format("\t| Xn: {0} NET: {1} Own: {2} Next: {3}|", this.Number, this.NextEventTime, this.Owner, this.NextOwner);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
