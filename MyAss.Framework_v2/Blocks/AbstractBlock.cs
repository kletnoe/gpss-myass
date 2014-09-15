using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework_v2.Blocks
{
    public abstract class AbstractBlock : IBlock
    {
        public int Id { get; protected set; }
        public int EntryCount { get; protected set; }
        public IBlock NextSequentialBlock { get; set; }
        public LinkedList<Transaction> RetryChain { get; protected set; }

        private string label;

        public string Label
        {
            get
            {
                return this.label;
            }
        }

        protected AbstractBlock()
        {
            this.EntryCount = 0;
            this.RetryChain = new LinkedList<Transaction>();
        }

        public void SetLabel(string label)
        {
            this.label = label;
        }

        public void SetId(int id)
        {
            this.Id = id;
        }

        public abstract void Action(Simulation simulation);

        public void PassTransaction(Transaction transaction)
        {
            transaction.NextOwner = this.Id;
        }

        public override string ToString()
        {
            return String.Format("Id: {0} Type: {1} Ent: {2}", this.Id, this.GetType().Name.Substring(0,3), this.EntryCount);
        }
    }
}
