using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework.OperandTypes;
using MyAss.Framework.Entities;

namespace MyAss.Framework.Blocks
{
    public class Enter : AbstractBlock
    {
        public IDoubleOperand A_StorageEntityId { get; private set; }
        public IDoubleOperand B_NumberOfUnits { get; private set; }

        public Enter(IDoubleOperand storageEntityId, IDoubleOperand numberOfUnits)
        {
            this.A_StorageEntityId = storageEntityId;
            this.B_NumberOfUnits = numberOfUnits;
        }

        public void PreEnter(Transaction transaction)
        {
            // TODO: Refactor this

            StorageEntity storage = (StorageEntity)Simulation.It.GetEntity((int)this.A_StorageEntityId.GetValue());
            int units = (int)this.B_NumberOfUnits.GetValue();

            if (storage.IsAvaliable && storage.RemainingCapacity >= units)
            {
                this.EntryCount++;

                storage.Enter(units);
                Console.WriteLine("Entered  \tTime: " + Simulation.It.Clock + transaction, ConsoleColor.Yellow);
                Console.WriteLine("\tStorageSize: " + storage.CurrentCount);
                transaction.Owner = this.Id;
                this.NextSequentialBlock.PassTransaction(transaction);
                Simulation.It.CurrentEventChain.AddAhead(transaction);
            }
            else
            {
                Console.WriteLine("preEntered  \tTime: " + Simulation.It.Clock + transaction, ConsoleColor.Yellow);
                transaction.NextEventTime = -1; //temp for tests
                storage.DelayChain.AddLast(transaction);
            }
            
        }

        public override void Action()
        {
            // TODO: Add Exception throwing

            Transaction transaction = Simulation.It.ActiveTransction;

            StorageEntity storage = (StorageEntity)Simulation.It.GetEntity((int)this.A_StorageEntityId.GetValue());
            int units = (int)this.B_NumberOfUnits.GetValue();

            if (storage.IsAvaliable && storage.RemainingCapacity >= units)
            {
                this.EntryCount++;

                storage.Enter(units);
                Console.WriteLine("Entered  \tTime: " + Simulation.It.Clock + transaction, ConsoleColor.Yellow);
                Console.WriteLine("\tStorageSize: " + storage.CurrentCount);
                transaction.Owner = this.Id;
                this.NextSequentialBlock.PassTransaction(transaction);
                Simulation.It.CurrentEventChain.AddAhead(transaction);
            }
            else
            {
                Console.WriteLine("preEntered  \tTime: " + Simulation.It.Clock + transaction, ConsoleColor.Yellow);
                transaction.NextEventTime = -1; //temp for tests
                storage.DelayChain.AddLast(transaction);
            }
            
        }
    }
}
