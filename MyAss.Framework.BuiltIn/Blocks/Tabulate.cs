using MyAss.Framework.Blocks;
using MyAss.Framework.BuiltIn.Entities;
using MyAss.Framework.OperandTypes;

namespace MyAss.Framework.BuiltIn.Blocks
{
    public class Tabulate : AnyBlock
    {
        public IDoubleOperand A_TableEntityId { get; private set; }
        public IDoubleOperand B_WeightingFactor { get; private set; }

        public Tabulate(IDoubleOperand tableEntityId, IDoubleOperand weightingFactor)
        {
            this.A_TableEntityId = tableEntityId;
            this.B_WeightingFactor = weightingFactor;
        }

        public override void Action()
        {
            // A: Required. The operand must be PosInteger.
            if (this.A_TableEntityId == null)
            {
                throw new ModelingException("TABULATE: Operand A is required operand!");
            }
            int entityId = (int)this.A_TableEntityId.GetValue();
            if (entityId <= 0)
            {
                throw new ModelingException("TABULATE: Operand A must be PosInteger!");
            }

            // B: The default is 1. The operand must be PosInteger.
            int weightedFactor = this.B_WeightingFactor == null ? 1 : (int)this.B_WeightingFactor.GetValue();
            if (weightedFactor <= 0)
            {
                throw new ModelingException("TABULATE: Operand B must be PosInteger!");
            }


            Transaction transaction = this.Simulation.ActiveTransaction;
            this.EntryCount++;

            TableEntity tableEntity = (TableEntity)this.Simulation.GetEntity(entityId);
            tableEntity.Tabulate(weightedFactor);

            //Console.WriteLine("Tabulated  \tTime: " + simulation.Clock + transaction, ConsoleColor.DarkGreen);
            transaction.ChangeOwner(this);
            this.NextSequentialBlock.PassTransaction(transaction);
            this.Simulation.CurrentEventChain.AddAhead(transaction);
        }
    }
}
