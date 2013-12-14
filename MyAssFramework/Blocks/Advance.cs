using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework.OperandTypes_Test;

namespace MyAssFramework.Blocks
{
    public class Advance : AbstractBlock
    {
        // TODO: Implement special "function modifier" case.

        public Operand A_MeanValue { get; private set; }
        public Operand B_HalfRange { get; private set; }

        public Advance(Operand meanValue, Operand halfRange)
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

            double increment = new Distributions.Uniform(0, meanValue - halfRange, meanValue + halfRange).GetNext();

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
                NextSequentialBlock.PassTransaction(transaction);
                Simulation.It.CurrentEventChain.AddAhead(transaction);
            }
            else
            {
                transaction.NextEventTime = nextTime;
                NextSequentialBlock.PassTransaction(transaction);
                Simulation.It.FutureEventChain.Add(transaction);
            }
        }
    }
}
