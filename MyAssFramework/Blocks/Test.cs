using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework.RelationalOp;
using MyAssFramework.OperandTypes_Test;
using MyAssFramework.Entities;

namespace MyAssFramework.Blocks
{
    public class Test : AbstractBlock
    {
        public RelationalOperator O_RelationalOp { get; private set; }

        public Operand A_LValue { get; private set; }
        public Operand B_RValue { get; private set; }
        public Operand C_DestBlockNo { get; private set; }

        public Test(RelationalOperator relationalOp, Operand lValue, Operand rValue, Operand destBlockNo)
        {
            this.O_RelationalOp = relationalOp;
            this.A_LValue = lValue;
            this.B_RValue = rValue;
            this.C_DestBlockNo = destBlockNo;
        }

        public override void Action()
        {
            // TODO: Add Exception throwing
            // TODO: Implement Refuse Mode

            Transaction transaction = Simulation.It.ActiveTransction;
            this.EntryCount++;

            Console.WriteLine("Tested  \tTime: " + Simulation.It.Clock + transaction, ConsoleColor.DarkGreen);
            Console.WriteLine("\ttrue");

            IBlock consumerOnFalse = Simulation.It.GetBlock((int)C_DestBlockNo.GetValue());

            double lValue = this.A_LValue.GetValue();
            double rValue = this.B_RValue.GetValue();

            if (O_RelationalOp.Compare(lValue, rValue))
            {
                this.NextSequentialBlock.PassTransaction(transaction);
            }
            else
            {
                consumerOnFalse.PassTransaction(transaction);
            }

            Simulation.It.CurrentEventChain.AddAhead(transaction);
        }
    }
}
