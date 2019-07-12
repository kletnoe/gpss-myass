using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework.Entities
{
    public interface IPendableEntity
    {
        LinkedList<Transaction> PendingChain { get; }
    }
}
