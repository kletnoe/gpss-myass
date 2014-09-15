using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework_v2.Blocks
{
    public interface IBlock
    {
        int Id { get; }
        string Label { get; }
        int EntryCount { get; }
        IBlock NextSequentialBlock { get; set; }
        LinkedList<Transaction> RetryChain { get; }

        void SetLabel(string label);

        void SetLabel(int id);

        void Action(Simulation simulation);

        void PassTransaction(Transaction transaction);
    }
}
