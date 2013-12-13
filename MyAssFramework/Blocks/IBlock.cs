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

        void Action();

        void PassTransaction(Transaction transaction);
    }
}
