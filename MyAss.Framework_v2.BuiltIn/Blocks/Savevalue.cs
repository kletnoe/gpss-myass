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
    public class Savevalue : AbstractBlock, IEntityCretableBlock
    {
        public IDoubleOperand A_SavevalueEntityId { get; private set; }
        public IDoubleOperand B_Value { get; private set; }

        public Savevalue(IDoubleOperand savevalueEntityId, IDoubleOperand value)
        {
            this.A_SavevalueEntityId = savevalueEntityId;
            this.B_Value = value;
        }

        public override void Action(Simulation simulation)
        {
            // A: Required.
            if (this.A_SavevalueEntityId == null)
            {
                throw new ModelingException("SAVEVALUE: Operand A is required operand!");
            }
            int entityId = (int)this.A_SavevalueEntityId.GetValue();

            // B: Required.
            // TODO: Savevalues currently operates with strings...
            string value = this.B_Value == null ? null : this.B_Value.GetValue().ToString();
            if (value == null)
            {
                throw new ModelingException("SAVEVALUE: Operand B is required operand!");
            }


            this.CreateEntity(simulation, entityId);

            Transaction transaction = simulation.ActiveTransaction;
            this.EntryCount++;

            SavevalueEntity savevalueEntity = (SavevalueEntity)simulation.GetEntity(entityId);

            savevalueEntity.SetValue(value);

            Console.WriteLine("Savevalued\tTime: " + simulation.Clock + transaction, ConsoleColor.White);
            transaction.Owner = this.Id;
            this.NextSequentialBlock.PassTransaction(transaction);
            simulation.CurrentEventChain.AddAhead(transaction);
        }

        public void CreateEntity(Simulation simulation, int entityId)
        {
            if (!simulation.Entities.ContainsKey(entityId))
            {
                SavevalueEntity savevalueEntity = new SavevalueEntity(simulation, entityId);
                simulation.Entities.Add(entityId, savevalueEntity);
            }
        }
    }
}
