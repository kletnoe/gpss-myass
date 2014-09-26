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

        // TODO: Implement Refuse Mode
        public override void Action(Simulation simulation)
        {
            // O: Required
            if (this.O_RelationalOp == null || this.O_RelationalOp.Value == null)
            {
                throw new ModelingException("TEST: Operand O is required conditional operand!");
            }
            string relationalOp = this.O_RelationalOp.Value;

            // A: Required.
            if (this.A_LValue == null)
            {
                throw new ModelingException("TEST: Operand A is required operand!");
            }
            double lValue = this.A_LValue.GetValue();

            // B: Required.
            if (this.B_RValue == null)
            {
                throw new ModelingException("TEST: Operand B is required operand!");
            }
            double rValue = this.B_RValue.GetValue();

            // C: Required. The operand must be PosInteger. Optional in refuse mode.
            if (this.C_DestBlockNo == null)
            {
                throw new ModelingException("TEST: Operand C is required operand!");
            }
            int consumerOnFalseBlockId = (int)C_DestBlockNo.GetValue();
            if (consumerOnFalseBlockId <= 0)
            {
                throw new ModelingException("TEST: Operand C must be PosInteger!");
            }


            Transaction transaction = simulation.ActiveTransaction;
            this.EntryCount++;

            Console.WriteLine("Tested  \tTime: " + simulation.Clock + transaction, ConsoleColor.DarkGreen);
            Console.WriteLine("\ttrue");

            IBlock consumerOnFalse = simulation.Blocks[consumerOnFalseBlockId];

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
                    throw new ModelingException("TEST: Operand O invalid LiteralOperand!");
            }
        }
    }
}
