using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework.Blocks;
using MyAss.Framework.OperandTypes;

namespace MyAss.Framework.BuiltIn.Blocks
{
    public class Terminate : AnyBlock
    {
        public IDoubleOperand A_TerminationCountDecriment { get; private set; }

        public Terminate(IDoubleOperand terminationCountDecriment)
        {
            this.A_TerminationCountDecriment = terminationCountDecriment;
        }

        public override void Action()
        {
            // A: The default is 0.
            int decriment = this.A_TerminationCountDecriment == null ? 0 : (int)this.A_TerminationCountDecriment.GetValue();

            Transaction transaction = this.Simulation.ActiveTransaction;
            this.EntryCount++;

            Console.WriteLine("Terminated\tTime: " + this.Simulation.Clock + transaction + " " + this.Id, ConsoleColor.DarkGray);
            transaction.ChangeOwner(this);
            this.Simulation.TerminationsCount -= decriment;

            this.Release(transaction);

            transaction.Dispose();
        }
    }
}
