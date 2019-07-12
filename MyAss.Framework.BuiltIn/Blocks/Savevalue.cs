using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework.OperandTypes;
using MyAss.Framework.Entities;
using MyAss.Framework.Blocks;
using MyAss.Framework.BuiltIn.Entities;

namespace MyAss.Framework.BuiltIn.Blocks
{
    public class Savevalue : AnyBlock, IEntityCretableBlock
    {
        public IDoubleOperand A_SavevalueEntityId { get; private set; }
        public IDoubleOperand B_Value { get; private set; }

        public Savevalue(IDoubleOperand savevalueEntityId, IDoubleOperand value)
        {
            this.A_SavevalueEntityId = savevalueEntityId;
            this.B_Value = value;
        }

        public override void Action()
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


            this.CreateEntity(entityId);

            Transaction transaction = this.Simulation.ActiveTransaction;
            this.EntryCount++;

            SavevalueEntity savevalueEntity = (SavevalueEntity)this.Simulation.GetEntity(entityId);

            savevalueEntity.SetValue(value);

            Console.WriteLine("Savevalued\tTime: " + this.Simulation.Clock + transaction, ConsoleColor.White);
            transaction.ChangeOwner(this);
            this.NextSequentialBlock.PassTransaction(transaction);
            this.Simulation.CurrentEventChain.AddAhead(transaction);
        }

        public void CreateEntity(int entityId)
        {
            if (!this.Simulation.Entities.ContainsKey(entityId))
            {
                SavevalueEntity savevalueEntity = new SavevalueEntity(this.Simulation, entityId);
                this.Simulation.Entities.Add(entityId, savevalueEntity);
            }
        }
    }
}
