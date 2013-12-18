using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework.OperandTypes_Test;
using MyAssFramework.Entities;

namespace MyAssFramework.Blocks
{
    public class Savevalue : AbstractBlock
    {
        public enum Operation
        {
            Plus,
            Minus,
            Set
        }

        public Operand A_SavevalueEntityId { get; private set; }
        public Savevalue.Operation A_Operation { get; private set; }
        public Operand B_Value { get; private set; }

        public Savevalue(Operand savevalueEntityId, Savevalue.Operation operation, Operand value)
        {
            this.A_SavevalueEntityId = savevalueEntityId;
            this.A_Operation = operation;
            this.B_Value = value;
        }

        public override void Action()
        {
            // TODO: Add Exception throwing

            Transaction transaction = Simulation.It.ActiveTransction;
            this.EntryCount++;

            SavevalueEntity savevalueEntity = (SavevalueEntity)Simulation.It.GetEntity((int)this.A_SavevalueEntityId.GetValue());
            string value = this.B_Value.GetValue().ToString();

            switch (A_Operation)
            {
                case Operation.Plus:
                    savevalueEntity.AddValue(value);
                    break;
                case Operation.Minus:
                    savevalueEntity.SubValue(value);
                    break;
                case Operation.Set:
                    savevalueEntity.SetValue(value);
                    break;
            }

            Console.WriteLine("Savevalued\tTime: " + Simulation.It.Clock + transaction, ConsoleColor.White);
            transaction.Owner = this.Id;
            this.NextSequentialBlock.PassTransaction(transaction);
            Simulation.It.CurrentEventChain.AddAhead(transaction);
        }
    }
}
