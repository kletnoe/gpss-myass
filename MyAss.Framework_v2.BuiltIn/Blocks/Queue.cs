using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.OperandTypes;
using MyAss.Framework_v2.BuiltIn.Entities;

namespace MyAss.Framework_v2.BuiltIn.Blocks
{
    public class Queue : AnyBlock, IEntityCretableBlock
    {
        public IDoubleOperand A_QueueEntityId { get; private set; }
        public IDoubleOperand B_NumberOfUnits { get; private set; }
        
        public Queue(IDoubleOperand queueEntityId, IDoubleOperand numberOfUnits)
        {
            this.A_QueueEntityId = queueEntityId;
            this.B_NumberOfUnits = numberOfUnits;
        }

        public override void Action()
        {
            // A: Required. The operand must be PosInteger.
            if (this.A_QueueEntityId == null)
            {
                throw new ModelingException("QUEUE: Operand A is required operand!");
            }
            int entityId = (int)this.A_QueueEntityId.GetValue();
            if (entityId <= 0)
            {
                throw new ModelingException("QUEUE: Operand A must be PosInteger!");
            }

            // B: The default is 1. The operand must be PosInteger.
            int units = this.B_NumberOfUnits == null ? 1 : (int)this.B_NumberOfUnits.GetValue();
            if (units <= 0)
            {
                throw new ModelingException("QUEUE: Operand B must be PosInteger!");
            }

            this.CreateEntity(entityId);

            Transaction transaction = this.Simulation.ActiveTransaction;
            this.EntryCount++;

            QueueEntity queue = (QueueEntity)this.Simulation.GetEntity(entityId);
            queue.Queue(units);

            Console.WriteLine("Queued  \tTime: " + this.Simulation.Clock + transaction, ConsoleColor.DarkYellow);
            Console.WriteLine("\tQueueSize: " + queue.CurrentContent);
            transaction.ChangeOwner(this);
            this.NextSequentialBlock.PassTransaction(transaction);
            this.Simulation.CurrentEventChain.AddAhead(transaction);
        }

        public void CreateEntity(int entityId)
        {
            if (!this.Simulation.Entities.ContainsKey(entityId))
            {
                QueueEntity queueEntity = new QueueEntity(this.Simulation, entityId);
                this.Simulation.Entities.Add(entityId, queueEntity);
            }
        }
    }
}
