using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.Entities
{
    public interface IEntity
    {
        int Id { get; }
        LinkedList<Transaction> RetryChain { get; }

        void UpdateStats();
    }
}
