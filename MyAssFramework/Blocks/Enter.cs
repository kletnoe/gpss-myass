using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework.OperandTypes_Test;
using MyAssFramework.Entities;

namespace MyAssFramework.Blocks
{
    public class Enter : AbstractBlock
    {
        public Operand A_StorageEntityId { get; private set; }
        public Operand B_NumberOfUnits { get; private set; }

        public Enter(Operand storageEntityId, Operand numberOfUnits)
        {
            this.A_StorageEntityId = storageEntityId;
            this.B_NumberOfUnits = numberOfUnits;
        }

        public override void Action()
        {
            // TODO: Add Exception throwing

            Transaction transaction = Simulation.ActiveTransction;

            StorageEntity storage = (StorageEntity)Simulation.GetEntity((int)this.A_StorageEntityId.GetValue());
            int units = (int)this.B_NumberOfUnits.GetValue();

            if (storage.IsAvaliable && storage.RemainingCapacity >= units)
            {
                this.EntryCount++;

                storage.Enter(units);
                Console.WriteLine("Entered  \tTime: " + Simulation.Clock + transaction, ConsoleColor.Yellow);
                System.Console.WriteLine("\tStorageSize: " + storage.CurrentCount);
                this.NextSequentialBlock.PassTransaction(transaction);
                Simulation.CurrentEventChain.AddAhead(transaction);
            }
            else
            {
                Console.WriteLine("preEntered  \tTime: " + Simulation.Clock + transaction, ConsoleColor.Yellow);
                transaction.NextEventTime = -1; //temp for tests
                storage.DelayChain.AddLast(transaction);
            }
            
        }
    }
}
