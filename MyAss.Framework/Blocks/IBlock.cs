using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework.Blocks
{
    public interface IBlock
    {
        int Id { get; }
        string Label { get; }
        int EntryCount { get; }
        IBlock NextSequentialBlock { get; set; }
        LinkedList<Transaction> RetryChain { get; }

        void SetLabel(string label);

        void Action();

        void PassTransaction(Transaction transaction);
    }
}
