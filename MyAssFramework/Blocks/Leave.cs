using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework.OperandTypes_Test;
using MyAssFramework.Entities;

namespace MyAssFramework.Blocks
{
    public class Leave : AbstractBlock
    {
        public Operand A_StorageEntityId { get; private set; }
        public Operand B_NumberOfUnits { get; private set; }

        public Leave(Operand storageEntityId, Operand numberOfUnits)
        {
            //this.Consumer = consumer;
            this.A_StorageEntityId = storageEntityId;
            this.B_NumberOfUnits = numberOfUnits;
        }

        public override void Action()
        {
            Transaction transaction = Simulation.It.ActiveTransction;
            this.EntryCount++;

            StorageEntity storage = (StorageEntity)Simulation.It.GetEntity((int)this.A_StorageEntityId.GetValue());
            int units = (int)this.B_NumberOfUnits.GetValue();
            storage.Leave(units);

            Console.WriteLine("Leaved  \tTime: " + Simulation.It.Clock + transaction, ConsoleColor.Yellow);
            System.Console.WriteLine("\tStorageSize: " + storage.CurrentCount);
            this.RetryChain.RemoveFirst();
            this.NextSequentialBlock.PassTransaction(transaction);
            Simulation.It.CurrentEventChain.AddAhead(transaction);
        }
    }
}
