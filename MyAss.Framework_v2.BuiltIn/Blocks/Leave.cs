﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework_v2.OperandTypes;
using MyAss.Framework_v2.Entities;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.BuiltIn.Entities;

namespace MyAss.Framework_v2.BuiltIn.Blocks
{
    public class Leave : AnyBlock
    {
        public IDoubleOperand A_StorageEntityId { get; private set; }
        public IDoubleOperand B_NumberOfUnits { get; private set; }

        public Leave(IDoubleOperand storageEntityId, IDoubleOperand numberOfUnits)
        {
            this.A_StorageEntityId = storageEntityId;
            this.B_NumberOfUnits = numberOfUnits;
        }

        public override void Action()
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


            Transaction transaction = this.Simulation.ActiveTransaction;
            this.EntryCount++;

            StorageEntity storage = (StorageEntity)this.Simulation.GetEntity(entityId);
            storage.Leave(units);

            // Temp
            //this.file.WriteLine(this.Simulation.Clock - transaction.TransactionParameters[10002]);
            //this.file.Flush();
            //

            Console.WriteLine("Leaved  \tTime: " + this.Simulation.Clock + transaction, ConsoleColor.Yellow);
            Console.WriteLine("\tStorageSize: " + storage.CurrentCount, ConsoleColor.White);
            transaction.ChangeOwner(this);
            this.NextSequentialBlock.PassTransaction(transaction);
            this.Simulation.CurrentEventChain.AddAhead(transaction);
        }

        System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\temp\MM5_s.txt");
    }
}
