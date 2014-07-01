using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework.OperandTypes;
using MyAss.Framework.Procedures;

namespace MyAss.Framework.Blocks
{
    public class Generate : AbstractBlock
    {
        // TODO: Implement special "function modifier" case.

        public IDoubleOperand A_MeanValue { get; private set; }
        public IDoubleOperand B_HalfRange { get; private set; }
        public IDoubleOperand C_StartDelay { get; private set; }
        public IDoubleOperand D_CreationLimit { get; private set; }
        public IDoubleOperand E_PriorityLevel { get; private set; }

        public Generate(IDoubleOperand meanValue, IDoubleOperand halfRange, IDoubleOperand startDelay,
            IDoubleOperand creationLimit, IDoubleOperand priorityLevel)
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

            double increment = Distributions.Uniform(0, meanValue - halfRange, meanValue + halfRange);
            if (this.EntryCount == 0)
            {
                increment += startDelay;
            }

            return Simulation.It.Clock + increment;
        }

        private void GenerateNext()
        {
            Transaction transaction = new Transaction()
            {
                NextOwner = this.Id,
                Priority = 0,
                NextEventTime = this.GetNextEventTime(),
                MarkTime = this.GetNextEventTime()
            };
            Simulation.It.FutureEventChain.Add(transaction);
        }

        public override void Action()
        {
            Transaction transaction = Simulation.It.ActiveTransction;
            this.EntryCount++;

            Console.WriteLine("Generated\tTime: " + Simulation.It.Clock + transaction);
            transaction.Owner = this.Id;
            NextSequentialBlock.PassTransaction(transaction);
            Simulation.It.CurrentEventChain.AddAhead(transaction);

            if (this.D_CreationLimit == null || this.EntryCount < this.D_CreationLimit.GetValue())
            {
                this.GenerateNext();
            }
        }
    }
}
