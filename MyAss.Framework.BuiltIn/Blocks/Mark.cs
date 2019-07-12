using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework.Blocks;
using MyAss.Framework.OperandTypes;

namespace MyAss.Framework.BuiltIn.Blocks
{
    public class Mark : AnyBlock
    {
        public IDoubleOperand A_ParameterId { get; private set; }

        public Mark(IDoubleOperand parameterId)
        {
            this.A_ParameterId = parameterId;
        }

        public override void Action()
        {
            // A: Optional. The operand must be PosInteger.

            Transaction transaction = this.Simulation.ActiveTransaction;
            this.EntryCount++;

            if (A_ParameterId == null)
            {
                transaction.MarkTime = this.Simulation.Clock;
            }
            else
            {
                int parameterId = (int)this.A_ParameterId.GetValue();
                if (parameterId <= 0)
                {
                    throw new ModelingException("MARK: Operand A must be PosInteger!");
                }

                transaction.TransactionParameters.Add(parameterId, this.Simulation.Clock);
            }

            Console.WriteLine("Marked\tTime: " + this.Simulation.Clock + transaction, ConsoleColor.Gray);
            transaction.ChangeOwner(this);
            this.NextSequentialBlock.PassTransaction(transaction);
            this.Simulation.CurrentEventChain.AddAhead(transaction);
        }
    }
}
