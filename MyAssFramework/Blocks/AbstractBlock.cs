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
        protected LinkedList<Transaction> RetryChain { get; set; }

        protected AbstractBlock()
        {
            this.Id = Simulation.It.NextBlockNo();
            this.EntryCount = 0;
            this.RetryChain = new LinkedList<Transaction>();
        }
        public abstract void Action();

        public void PassTransaction(Transaction transaction)
        {
            transaction.Owner = this;
            this.RetryChain.AddLast(transaction);
        }
    }
}
