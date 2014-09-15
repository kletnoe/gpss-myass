using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.OperandTypes;

namespace MyAss.Framework_v2.BuiltIn.Blocks
{
    public class Advance : AbstractBlock
    {
        // TODO: Implement special "function modifier" case.

        public IDoubleOperand A_MeanValue { get; private set; }
        public IDoubleOperand B_HalfRange { get; private set; }

        private Random rand = new Random();

        public Advance(IDoubleOperand meanValue, IDoubleOperand halfRange)
        {
            this.A_MeanValue = meanValue;
            this.B_HalfRange = halfRange;
        }

        // When Operand B is an FN class SNA, it is a special case called a "function modifier". 
        // In this case, the time increment is calculated by multiplying the result of the function by the evaluated A Operand.
        private double GetTimeIncrement(double meanValue, double halfRange)
        {
            if ((meanValue - halfRange) < 0)
            {
                throw new ModelingException("ADVANCE: Negative time increment!");
            }

            double increment = meanValue - halfRange + (this.rand.NextDouble() * halfRange);

            return increment;
        }

        public override void Action(Simulation simualtion)
        {
            // A: The default is 0.
            double meanValue = this.A_MeanValue == null ? 0 : this.A_MeanValue.GetValue();

            // B: The default is 0.
            double halfRange = this.B_HalfRange == null ? 0 : this.B_HalfRange.GetValue();


            Transaction transaction = simualtion.ActiveTransaction;
            this.EntryCount++;

            Console.WriteLine("Advanced  \tTime: " + simualtion.Clock + transaction, ConsoleColor.DarkGreen);

            double nextTime = simualtion.Clock + this.GetTimeIncrement(meanValue, halfRange);

            if (transaction.NextEventTime == nextTime)
            {
                transaction.Owner = this.Id;
                NextSequentialBlock.PassTransaction(transaction);
                simualtion.CurrentEventChain.AddAhead(transaction);
            }
            else
            {
                transaction.Owner = this.Id;
                transaction.NextEventTime = nextTime;
                NextSequentialBlock.PassTransaction(transaction);
                simualtion.FutureEventChain.Add(transaction);
            }
        }
    }
}
