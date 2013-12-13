using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework.OperandTypes_Test;

namespace MyAssFramework.Blocks
{
    public class Terminate : AbstractBlock
    {
        public Operand A_TerminationCountDecriment { get; private set; }

        public Terminate(Operand terminationCountDecriment)
        {
            this.A_TerminationCountDecriment = terminationCountDecriment;
        }

        public override void Action()
        {
            Transaction transaction = Simulation.ActiveTransction;
            this.EntryCount++;

            Console.WriteLine("Terminated\tTime: " + Simulation.Clock + transaction + " " + this.Id, ConsoleColor.DarkGray);

            int decriment = (int)this.A_TerminationCountDecriment.GetValue();
            Simulation.TerminationsCount -= decriment;

            transaction.Dispose();
        }
    }
}
