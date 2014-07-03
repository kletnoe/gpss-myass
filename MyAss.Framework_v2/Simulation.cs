using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.Chains;
using MyAss.Framework_v2.Commands;
using MyAss.Framework_v2.Entities;

namespace MyAss.Framework_v2
{
    public sealed class Simulation
    {
        internal SimulationNumbersManager NumbersManager { get; private set; }
        internal PriorityTransactionChain CurrentEventChain { get; private set; }
        internal FutureTransactionChain FutureEventChain { get; private set; }
        internal IList<IEntity> Entities { get; private set; }
        internal Queue<ICommand> CommandQueue;

        internal IDictionary<int, IBlock> Blocks { get; private set; }

        internal double Clock { get; set; }
        internal int TerminationsCount { get; private set; }
        internal Transaction ActiveTransaction { get; set; }

        public Simulation()
        {
            this.NumbersManager = new SimulationNumbersManager();
            this.CurrentEventChain = new PriorityTransactionChain();
            this.FutureEventChain = new FutureTransactionChain();
        }
    }
}
