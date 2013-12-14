using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.Blocks
{
    public interface IBlock
    {
        int Id { get; }
        int EntryCount { get; }
        IBlock NextSequentialBlock { get; set; }
        LinkedList<Transaction> RetryChain { get; }
        void Action();

        void PassTransaction(Transaction transaction);
    }
}
