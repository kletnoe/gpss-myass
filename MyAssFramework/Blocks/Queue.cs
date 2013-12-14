using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework.OperandTypes_Test;
using MyAssFramework.Entities;

namespace MyAssFramework.Blocks
{
    public class Queue : AbstractBlock
    {
        public Operand A_QueueEntityId { get; private set; }
        public Operand B_NumberOfUnits { get; private set; }
        
        public Queue(Operand queueEntityId, Operand numberOfUnits)
        {
            this.A_QueueEntityId = queueEntityId;
            this.B_NumberOfUnits = numberOfUnits;
        }

        public override void Action()
        {
            // TODO: Add Exception throwing

            Transaction transaction = Simulation.It.ActiveTransction;
            this.EntryCount++;

            QueueEntity queue = (QueueEntity)Simulation.It.GetEntity((int)this.A_QueueEntityId.GetValue());
            int units = (int)this.B_NumberOfUnits.GetValue();
            queue.Queue(units);

            Console.WriteLine("Queued  \tTime: " + Simulation.It.Clock + transaction, ConsoleColor.DarkYellow);
            System.Console.WriteLine("\tQueueSize: " + queue.CurrentContent);
            this.RetryChain.RemoveFirst();
            this.NextSequentialBlock.PassTransaction(transaction);
            Simulation.It.CurrentEventChain.AddAhead(transaction);
        }
    }
}
