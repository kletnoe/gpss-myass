using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework.OperandTypes_Test;
using MyAss.Framework.Entities;

namespace MyAss.Framework.Blocks
{
    public class Leave : AbstractBlock
    {
        public IDoubleOperand A_StorageEntityId { get; private set; }
        public IDoubleOperand B_NumberOfUnits { get; private set; }

        public Leave(IDoubleOperand storageEntityId, IDoubleOperand numberOfUnits)
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
            Console.WriteLine("\tStorageSize: " + storage.CurrentCount);
            transaction.Owner = this.Id;
            this.NextSequentialBlock.PassTransaction(transaction);
            Simulation.It.CurrentEventChain.AddAhead(transaction);
        }
    }
}
