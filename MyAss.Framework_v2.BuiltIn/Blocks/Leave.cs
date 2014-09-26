using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework_v2.OperandTypes;
using MyAss.Framework_v2.Entities;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.BuiltIn.Entities;

namespace MyAss.Framework_v2.BuiltIn.Blocks
{
    public class Leave : AbstractBlock
    {
        public IDoubleOperand A_StorageEntityId { get; private set; }
        public IDoubleOperand B_NumberOfUnits { get; private set; }

        public Leave(IDoubleOperand storageEntityId, IDoubleOperand numberOfUnits)
        {
            this.A_StorageEntityId = storageEntityId;
            this.B_NumberOfUnits = numberOfUnits;
        }

        public override void Action(Simulation simulation)
        {
            // A: Required. The operand must be PosInteger.
            if (this.A_StorageEntityId == null)
            {
                throw new ModelingException("LEAVE: Operand A is required operand!");
            }
            int entityId = (int)this.A_StorageEntityId.GetValue();
            if (entityId <= 0)
            {
                throw new ModelingException("LEAVE: Operand A must be PosInteger!");
            }

            // B: The default is 1. The operand must be PosInteger.
            int units = this.B_NumberOfUnits == null ? 1 : (int)this.B_NumberOfUnits.GetValue();
            if (units <= 0)
            {
                throw new ModelingException("LEAVE: Operand B must be PosInteger!");
            }


            Transaction transaction = simulation.ActiveTransaction;
            this.EntryCount++;

            StorageEntity storage = (StorageEntity)simulation.GetEntity(entityId);
            storage.Leave(units);

            Console.WriteLine("Leaved  \tTime: " + simulation.Clock + transaction, ConsoleColor.Yellow);
            Console.WriteLine("\tStorageSize: " + storage.CurrentCount);
            transaction.Owner = this.Id;
            this.NextSequentialBlock.PassTransaction(transaction);
            simulation.CurrentEventChain.AddAhead(transaction);
        }
    }
}
