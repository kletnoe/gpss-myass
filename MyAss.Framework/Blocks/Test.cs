using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework.RelationalOp;
using MyAss.Framework.OperandTypes_Test;
using MyAss.Framework.Entities;

namespace MyAss.Framework.Blocks
{
    public class Test : AbstractBlock
    {
        public RelationalOperator O_RelationalOp { get; private set; }

        public IDoubleOperand A_LValue { get; private set; }
        public IDoubleOperand B_RValue { get; private set; }
        public IDoubleOperand C_DestBlockNo { get; private set; }

        public Test(RelationalOperator relationalOp, IDoubleOperand lValue, IDoubleOperand rValue, IDoubleOperand destBlockNo)
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
                transaction.Owner = this.Id;
                this.NextSequentialBlock.PassTransaction(transaction);
            }
            else
            {
                transaction.Owner = this.Id;
                consumerOnFalse.PassTransaction(transaction);
            }

            Simulation.It.CurrentEventChain.AddAhead(transaction);
        }
    }
}
