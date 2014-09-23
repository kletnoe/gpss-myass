using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.OperandTypes;

namespace MyAss.Framework_v2.BuiltIn.Blocks
{
    public class Mark : AbstractBlock
    {
        public IDoubleOperand A_ParameterId { get; private set; }

        public Mark(IDoubleOperand parameterId)
        {
            this.A_ParameterId = parameterId;
        }

        public override void Action(Simulation simulation)
        {
            // A: Optional.

            Transaction transaction = simulation.ActiveTransaction;
            this.EntryCount++;

            if (A_ParameterId == null)
            {
                transaction.MarkTime = simulation.Clock;
            }
            else
            {
                int parameterId = (int)this.A_ParameterId.GetValue();
                transaction.TransactionParameters.Add(parameterId, simulation.Clock);
            }

            Console.WriteLine("Marked\tTime: " + simulation.Clock + transaction, ConsoleColor.Gray);
            transaction.Owner = this.Id;
            this.NextSequentialBlock.PassTransaction(transaction);
            simulation.CurrentEventChain.AddAhead(transaction);
        }
    }
}
