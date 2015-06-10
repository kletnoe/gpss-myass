using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using MyAss.Framework_v2.Blocks;

namespace MyAss.Framework_v2
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

        public AnyBlock Owner { get; private set; }
        public AnyBlock NextOwner { get; private set; }

        public double NextEventTime { get; set; }

        /**/

        private Dictionary<int, double> transactionParameters;

        public Dictionary<int, double> TransactionParameters
        {
            get
            {
                if (this.transactionParameters == null)
                {
                    this.transactionParameters = new Dictionary<int, double>();
                }

                return transactionParameters;
            }
        }

        public int Priority { get; set; }

        public double MarkTime { get; set; }

        public int Number { get; private set; }

        public int AssemblySet { get; set; }

        public bool DelayIndicator { get; private set; }

        public bool TraceIndicator { get; private set; }

        public TransactionState State { get; private set; }

        public bool IsPreempted { get; private set; }

        public Transaction(int transactionNo, double creationTime)
        {
            int number = transactionNo;
            this.AssemblySet = number;
            this.Number = number;

            this.Priority = 0;
            this.MarkTime = creationTime;
            this.DelayIndicator = false;
            this.TraceIndicator = false;
            this.State = TransactionState.Active;
            this.IsPreempted = false;
        }

        public Transaction(int transactionNo, double creationTime, int priority, AnyBlock ownerId) : this(transactionNo, creationTime)
        {
            this.Priority = priority;
            this.Owner = ownerId;
        }

        public override string ToString()
        {
            // return String.Format("Time: {0}, Prio: {1}, Clock: {2} ", CreationRealTime.ToString("HH:mm:ss.fff"), Priority, GenerationTime);
            return String.Format("\t| Xn: {0} NET: {1} Own: {2} Next: {3}|", this.Number, this.NextEventTime.ToString("F5"), this.Owner, this.NextOwner);
        }

        public void ChangeOwner(AnyBlock newOwner)
        {
            if (this.Owner != null)
            {
                this.Owner.Release(this);
            }

            newOwner.Own(this);
            this.Owner = newOwner;
        }

        public void SetNextOwner(AnyBlock newNextOwner)
        {
            this.NextOwner = newNextOwner;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Transaction other = obj as Transaction;
            if ((System.Object)other == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.Number == other.Number;
        }

        public override int GetHashCode()
        {
            return this.Number;
        }

        public Transaction Split(int transactionNo)
        {
            Transaction offspring = new Transaction(transactionNo, this.MarkTime);

            offspring.AssemblySet = this.AssemblySet;
            offspring.DelayIndicator = this.DelayIndicator;
            offspring.IsPreempted = this.IsPreempted;
            offspring.NextEventTime = this.NextEventTime;
            offspring.NextOwner = this.NextOwner;
            offspring.Owner = this.Owner;
            offspring.Priority = this.Priority;
            offspring.State = this.State;
            offspring.TraceIndicator = this.TraceIndicator;

            if (this.transactionParameters != null)
            {
                offspring.transactionParameters = new Dictionary<int, double>();
                foreach (var parameter in this.transactionParameters)
                {
                    offspring.transactionParameters.Add(parameter.Key, parameter.Value);
                }
            }

            return offspring;
        }
    }
}
