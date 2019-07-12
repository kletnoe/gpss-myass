using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework.Entities
{
    public abstract class AnyEntity
    {
        public int Id { get; protected set; }
        public LinkedList<Transaction> RetryChain { get; protected set; }

        protected AnyEntity()
        {
            this.RetryChain = new LinkedList<Transaction>();
        }

        public abstract void UpdateStats();
        public abstract string GetStandardReportHeader();
        public abstract string GetStandardReportLine();
    }
}
