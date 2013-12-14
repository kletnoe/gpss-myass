using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using MyAssFramework.Blocks;

namespace MyAssFramework
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

        public IBlock Owner { get; set; }

        public double NextEventTime { get; set; }

        /**/

        public OrderedDictionary Parameters;

        public double Priority { get; set; }

        public double MarkTime { get; private set; }

        public int AssemblySet { get; private set; }

        public bool DelayIndicator { get; private set; }

        public bool TraceIndicator { get; private set; }

        //public int CurrentBlock { get; private set; }

        //public int NextBlock { get; private set; }

        public TransactionState State { get; private set; }

        public bool IsPreempted { get; private set; }

        public Transaction()
        {
            this.Priority = 0;
            this.MarkTime = Simulation.It.Clock;
            this.AssemblySet = Simulation.It.NextTransactionNo();
            this.DelayIndicator = false;
            this.TraceIndicator = false;
            //this.CurrentBlock = 0;
            //this.NextBlock = 0;
            this.State = TransactionState.Active;
            this.IsPreempted = false;
        }

        public Transaction(double priority, IBlock owner) : this()
        {
            this.Priority = priority;
            this.Owner = owner;
        }

        public override string ToString()
        {
            // return String.Format("Time: {0}, Prio: {1}, Clock: {2} ", CreationRealTime.ToString("HH:mm:ss.fff"), Priority, GenerationTime);
            return String.Format("\t| Id: {1} net: {2} own: {3}|", this.Priority, this.AssemblySet, this.NextEventTime, this.Owner.GetType().Name);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
