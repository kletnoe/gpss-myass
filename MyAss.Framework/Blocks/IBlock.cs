using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework.Blocks
{
    public interface IBlock
    {
        int Id { get; }
        string Label { get; set; }
        int EntryCount { get; }
        IBlock NextSequentialBlock { get; set; }
        LinkedList<Transaction> RetryChain { get; }
        void Action();

        void PassTransaction(Transaction transaction);
    }
}
