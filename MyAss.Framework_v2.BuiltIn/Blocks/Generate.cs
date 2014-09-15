using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.OperandTypes;

namespace MyAss.Framework_v2.BuiltIn.Blocks
{
    public class Generate : AbstractBlock, ITransactionGeneratableBlock
    {
        // TODO: Implement special "function modifier" case.

        public IDoubleOperand A_MeanValue { get; private set; }
        public IDoubleOperand B_HalfRange { get; private set; }
        public IDoubleOperand C_StartDelay { get; private set; }
        public IDoubleOperand D_CreationLimit { get; private set; }
        public IDoubleOperand E_PriorityLevel { get; private set; }

        private Random rand = new Random();

        public Generate(IDoubleOperand meanValue, IDoubleOperand halfRange, IDoubleOperand startDelay,
            IDoubleOperand creationLimit, IDoubleOperand priorityLevel)
        {
            this.A_MeanValue = meanValue;
            this.B_HalfRange = halfRange;
            this.C_StartDelay = startDelay;
            this.D_CreationLimit = creationLimit;
            this.E_PriorityLevel = priorityLevel;
        }

        private double GetNextEventTime(Simulation simulation)
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

            double increment = meanValue - halfRange + (this.rand.NextDouble() * halfRange);
            if (this.EntryCount == 0)
            {
                increment += startDelay;
            }

            return simulation.Clock + increment;
        }

        private void GenerateNext(Simulation simulation)
        {
            double transactGenerationTime = this.GetNextEventTime(simulation);

            Transaction transaction = new Transaction(simulation.NumbersManager.NextFreeTransactionNo, simulation.Clock)
            {
                NextOwner = this.Id,
                Priority = 0,
                NextEventTime = transactGenerationTime,
                MarkTime = transactGenerationTime
            };
            simulation.FutureEventChain.Add(transaction);
        }

        public override void Action(Simulation simulation)
        {
            Transaction transaction = simulation.ActiveTransaction;
            this.EntryCount++;

            Console.WriteLine("Generated\tTime: " + simulation.Clock + transaction);
            transaction.Owner = this.Id;
            this.NextSequentialBlock.PassTransaction(transaction);
            simulation.CurrentEventChain.AddAhead(transaction);

            if (this.D_CreationLimit == null || this.EntryCount < this.D_CreationLimit.GetValue())
            {
                this.GenerateNext(simulation);
            }
        }

        public void GenerateFirst(Simulation simulation)
        {
            this.GenerateNext(simulation);
        }
    }
}
