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
        int EntryCount { get; }
        IBlock NextSequentialBlock { get; set; }
        LinkedList<Transaction> RetryChain { get; }

        void SetId(int id);

        void Action(Simulation simulation);

        void PassTransaction(Transaction transaction);
    }
}
