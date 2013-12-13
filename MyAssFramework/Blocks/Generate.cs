using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework.OperandTypes_Test;

namespace MyAssFramework.Blocks
{
    public class Generate : AbstractBlock
    {
        // TODO: Implement special "function modifier" case.

        public Operand A_MeanValue { get; private set; }
        public Operand B_HalfRange { get; private set; }
        public Operand C_StartDelay { get; private set; }
        public Operand D_CreationLimit { get; private set; }
        public Operand E_PriorityLevel { get; private set; }

        public Generate(Operand meanValue, Operand halfRange, Operand startDelay,
            Operand creationLimit, Operand priorityLevel)
        {
            this.A_MeanValue = meanValue;
            this.B_HalfRange = halfRange;
            this.C_StartDelay = startDelay;
            this.D_CreationLimit = creationLimit;
            this.E_PriorityLevel = priorityLevel;

            this.GenerateNext();
        }

        private double GetNextEventTime()
        {
            // When Operand B is an FN class SNA, it is a special case called a "function modifier". 
            // In this case, the time increment is calculated by multiplying the result of the function by the evaluated A Operand.

            if (this.A_MeanValue == null && this.D_CreationLimit == null)
            {
                throw new ModelingException("GENERATE: Either Operand A or Operand D must be used");
            }

            double meanValue = this.A_MeanValue == null ? 0 : this.A_MeanValue.GetValue();
            double halfRange = this.B_HalfRange == null ? 0 : this.B_HalfRange.GetValue();
            double startDelay = this.C_StartDelay == null ? 0 : this.C_StartDelay.GetValue();

            if ((meanValue - halfRange) < 0)
            {
                throw new ModelingException("GENERATE: Negative time increment.");
            }

            double increment = new Distributions.Uniform(0, meanValue - halfRange, meanValue + halfRange).GetNext();
            if (this.EntryCount == 0)
            {
                increment += startDelay;
            }

            return Simulation.Clock + increment;
        }

        private void GenerateNext()
        {
            Transaction transaction = new Transaction()
            {
                Owner = this,
                Priority = 0,
                NextEventTime = this.GetNextEventTime()
            };
            this.RetryChain.AddLast(transaction);
            Simulation.FutureEventChain.Add(transaction);
        }

        public override void Action()
        {
            Transaction transaction = Simulation.ActiveTransction;
            this.EntryCount++;

            Console.WriteLine("Generated\tTime: " + Simulation.Clock + transaction);
            this.RetryChain.RemoveFirst();
            NextSequentialBlock.PassTransaction(transaction);
            Simulation.CurrentEventChain.AddAhead(transaction);

            if (this.D_CreationLimit == null || this.EntryCount < this.D_CreationLimit.GetValue())
            {
                this.GenerateNext();
            }
        }
    }
}
