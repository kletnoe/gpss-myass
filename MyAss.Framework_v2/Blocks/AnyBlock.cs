using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework_v2.Blocks
{
    public abstract class AnyBlock
    {
        public int Id { get; protected set; }
        public string Label { get; protected set; }
        public Simulation Simulation { get; protected set; }

        public int EntryCount { get; protected set; }
        public int CurrentCount { get; protected set; }
        public AnyBlock NextSequentialBlock { get; set; }
        public LinkedList<Transaction> RetryChain { get; protected set; }

        protected AnyBlock()
        {
            this.EntryCount = 0;
            this.CurrentCount = 0;
            this.RetryChain = new LinkedList<Transaction>();
        }

        public void SetLabel(string label)
        {
            this.Label = label;
        }

        public void SetId(int id)
        {
            this.Id = id;
        }

        public void SetSimulation(Simulation simulation)
        {
            this.Simulation = simulation;
        }

        public void Own(Simulation simulation, Transaction transaction)
        {
            this.ActionOnOwn(simulation, transaction);
            this.CurrentCount++;
        }

        public void Release(Simulation simulation, Transaction transaction)
        {
            this.ActionOnDisown(simulation, transaction);
            this.CurrentCount--;
        }

        public virtual void ActionOnOwn(Simulation simulation, Transaction transaction)
        {

        }

        public virtual void ActionOnDisown(Simulation simulation, Transaction transaction)
        {

        }

        public abstract void Action(Simulation simulation);

        public void PassTransaction(Transaction transaction)
        {
            transaction.SetNextOwner(this);
        }

        public override string ToString()
        {
            return String.Format("Id: {0} Type: {1} Ent: {2}", this.Id, this.GetType().Name.Substring(0,3), this.EntryCount);
        }
    }
}
