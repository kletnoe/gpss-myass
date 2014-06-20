using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework.OperandTypes_Test;

namespace MyAssFramework.Blocks
{
    public class Mark : AbstractBlock
    {
        public IDoubleOperand A_ParameterId { get; private set; }

        public Mark(IDoubleOperand parameterId)
        {
            this.A_ParameterId = parameterId;
        }

        public override void Action()
        {
            Transaction transaction = Simulation.It.ActiveTransction;
            this.EntryCount++;

            if (A_ParameterId == null)
            {
                transaction.MarkTime = Simulation.It.Clock;
            }
            else
            {
                int parameterId = (int)this.A_ParameterId.GetValue();
                transaction.Parameters.SetParameter(parameterId, Simulation.It.Clock);
            }

            Console.WriteLine("Marked\tTime: " + Simulation.It.Clock + transaction, ConsoleColor.Gray);
            transaction.Owner = this.Id;
            this.NextSequentialBlock.PassTransaction(transaction);
            Simulation.It.CurrentEventChain.AddAhead(transaction);
        }
    }
}
