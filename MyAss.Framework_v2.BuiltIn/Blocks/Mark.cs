using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.OperandTypes;

namespace MyAss.Framework_v2.BuiltIn.Blocks
{
    public class Mark : AnyBlock
    {
        public IDoubleOperand A_ParameterId { get; private set; }

        public Mark(IDoubleOperand parameterId)
        {
            this.A_ParameterId = parameterId;
        }

        public override void Action(Simulation simulation)
        {
            // A: Optional. The operand must be PosInteger.

            Transaction transaction = simulation.ActiveTransaction;
            this.EntryCount++;

            if (A_ParameterId == null)
            {
                transaction.MarkTime = simulation.Clock;
            }
            else
            {
                int parameterId = (int)this.A_ParameterId.GetValue();
                if (parameterId <= 0)
                {
                    throw new ModelingException("MARK: Operand A must be PosInteger!");
                }

                transaction.TransactionParameters.Add(parameterId, simulation.Clock);
            }

            Console.WriteLine("Marked\tTime: " + simulation.Clock + transaction, ConsoleColor.Gray);
            transaction.ChangeOwner(simulation, this);
            this.NextSequentialBlock.PassTransaction(transaction);
            simulation.CurrentEventChain.AddAhead(transaction);
        }
    }
}
