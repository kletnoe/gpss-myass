using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework.OperandTypes;
using MyAss.Framework.Procedures;

namespace MyAss.Framework.Blocks
{
    public class Advance : AbstractBlock
    {
        // TODO: Implement special "function modifier" case.

        public IDoubleOperand A_MeanValue { get; private set; }
        public IDoubleOperand B_HalfRange { get; private set; }

        public Advance(IDoubleOperand meanValue, IDoubleOperand halfRange)
        {
            this.A_MeanValue = meanValue;
            this.B_HalfRange = halfRange;
        }

        private double GetNextEventTime()
        {
            // When Operand B is an FN class SNA, it is a special case called a "function modifier". 
            // In this case, the time increment is calculated by multiplying the result of the function by the evaluated A Operand.

            double meanValue = this.A_MeanValue == null ? 0 : this.A_MeanValue.GetValue();
            double halfRange = this.B_HalfRange == null ? 0 : this.B_HalfRange.GetValue();

            if ((meanValue - halfRange) < 0)
            {
                throw new ModelingException("ADVANCE: Negative time increment.");
            }

            double increment = Distributions.Uniform(0, meanValue - halfRange, meanValue + halfRange);

            return Simulation.It.Clock + increment;
        }

        public override void Action()
        {
            Transaction transaction = Simulation.It.ActiveTransction;
            this.EntryCount++;

            Console.WriteLine("Advanced  \tTime: " + Simulation.It.Clock + transaction, ConsoleColor.DarkGreen);

            double nextTime = this.GetNextEventTime();

            if (transaction.NextEventTime == nextTime)
            {
                transaction.Owner = this.Id;
                NextSequentialBlock.PassTransaction(transaction);
                Simulation.It.CurrentEventChain.AddAhead(transaction);
            }
            else
            {
                transaction.Owner = this.Id;
                transaction.NextEventTime = nextTime;
                NextSequentialBlock.PassTransaction(transaction);
                Simulation.It.FutureEventChain.Add(transaction);
            }
        }
    }
}
