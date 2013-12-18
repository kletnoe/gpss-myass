using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.Blocks
{
    public abstract class AbstractBlock : IBlock
    {
        public int Id { get; protected set; }
        public int EntryCount { get; protected set; }
        public IBlock NextSequentialBlock { get; set; }
        public LinkedList<Transaction> RetryChain { get; protected set; }

        protected AbstractBlock()
        {
            this.Id = Simulation.It.NextBlockNo();
            this.EntryCount = 0;
            this.RetryChain = new LinkedList<Transaction>();
        }
        public abstract void Action();

        public void PassTransaction(Transaction transaction)
        {
            transaction.NextOwner = this.Id;
        }

        public string ToString()
        {
            return String.Format("Id: {0} Type: {1} Ent: {2}", this.Id, this.GetType().Name.Substring(0,3), this.EntryCount);
        }
    }
}
