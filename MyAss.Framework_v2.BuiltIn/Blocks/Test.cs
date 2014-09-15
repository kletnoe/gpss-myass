using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework_v2.OperandTypes;
using MyAss.Framework_v2.Entities;
using MyAss.Framework_v2.Blocks;

namespace MyAss.Framework_v2.BuiltIn.Blocks
{
    public class Test : AbstractBlock
    {
        public LiteralOperand O_RelationalOp { get; private set; }

        public IDoubleOperand A_LValue { get; private set; }
        public IDoubleOperand B_RValue { get; private set; }
        public IDoubleOperand C_DestBlockNo { get; private set; }

        public Test(LiteralOperand relationalOp, IDoubleOperand lValue, IDoubleOperand rValue, IDoubleOperand destBlockNo)
        {
            this.O_RelationalOp = relationalOp;
            this.A_LValue = lValue;
            this.B_RValue = rValue;
            this.C_DestBlockNo = destBlockNo;
        }

        public override void Action(Simulation simulation)
        {
            // TODO: Add Exception throwing
            // TODO: Implement Refuse Mode

            Transaction transaction = simulation.ActiveTransaction;
            this.EntryCount++;

            Console.WriteLine("Tested  \tTime: " + simulation.Clock + transaction, ConsoleColor.DarkGreen);
            Console.WriteLine("\ttrue");

            IBlock consumerOnFalse = simulation.Blocks[(int)C_DestBlockNo.GetValue()];

            double lValue = this.A_LValue.GetValue();
            double rValue = this.B_RValue.GetValue();

            string relationalOp = this.O_RelationalOp.Value;

            if (this.Compare(relationalOp, lValue, rValue))
            {
                transaction.Owner = this.Id;
                this.NextSequentialBlock.PassTransaction(transaction);
            }
            else
            {
                transaction.Owner = this.Id;
                consumerOnFalse.PassTransaction(transaction);
            }

            simulation.CurrentEventChain.AddAhead(transaction);
        }

        private bool Compare(string relationalOp, double lValue, double rValue)
        {
            switch (relationalOp.ToUpperInvariant())
            {
                case "E": return (lValue == rValue);
                case "G": return (lValue > rValue);
                case "GE": return (lValue >= rValue);
                case "L": return (lValue < rValue);
                case "LE": return (lValue <= rValue);
                case "NE": return (lValue != rValue);
                default:
                    throw new ModelingException("Invalid TEST block LiteralOperand!");
            }
        }
    }
}
