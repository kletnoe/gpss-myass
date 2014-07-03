using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework_v2.Blocks
{
    public interface IBlock
    {
        void SetLabel(string label);

        void Action(Simulation simulation);

        void PassTransaction(Transaction transaction);
    }
}
