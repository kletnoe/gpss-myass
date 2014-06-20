using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework.Entities
{
    public abstract class AbstractEntity : IEntity
    {
        public int Id { get; protected set; }
        public LinkedList<Transaction> RetryChain { get; protected set; }

        protected AbstractEntity()
        {
            this.RetryChain = new LinkedList<Transaction>();
        }

        public abstract void UpdateStats();
    }
}
