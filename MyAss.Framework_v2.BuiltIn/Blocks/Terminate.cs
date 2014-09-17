using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.OperandTypes;

namespace MyAss.Framework_v2.BuiltIn.Blocks
{
    public class Terminate : AbstractBlock
    {
        public IDoubleOperand A_TerminationCountDecriment { get; private set; }

        public Terminate(IDoubleOperand terminationCountDecriment)
        {
            this.A_TerminationCountDecriment = terminationCountDecriment;
        }

        public override void Action(Simulation simulation)
        {
            // A: The default is 0.
            int decriment = this.A_TerminationCountDecriment == null ? 0 : (int)this.A_TerminationCountDecriment.GetValue();

            Transaction transaction = simulation.ActiveTransaction;
            this.EntryCount++;

            Console.WriteLine("Terminated\tTime: " + simulation.Clock + transaction + " " + this.Id, ConsoleColor.DarkGray);

            simulation.TerminationsCount -= decriment;
            transaction.Dispose();
        }
    }
}
