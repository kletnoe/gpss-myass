using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.OperandTypes;

namespace MyAss.Framework_v2.BuiltIn.Blocks
{
    public class Generate : AnyBlock, ITransactionGeneratableBlock
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

        // When Operand B is an FN class SNA, it is a special case called a "function modifier". 
        // In this case, the time increment is calculated by multiplying the result of the function by the evaluated A Operand.
        private Transaction GenerateNext()
        {
            // A: The default is 0. Either Operand A or Operand D must be used.
            double meanValue = this.A_MeanValue == null ? 0 : this.A_MeanValue.GetValue();

            // B: The default is 0.
            double halfRange = this.B_HalfRange == null ? 0 : this.B_HalfRange.GetValue();

            // C: The default is 0.
            double startDelay = this.C_StartDelay == null ? 0 : this.C_StartDelay.GetValue();

            // D: The default is Infinity. Either Operand A or Operand D must be used.
            //int creationLimit = this.D_CreationLimit == null ? Int32.MaxValue : (int)this.D_CreationLimit.GetValue();

            // E: The default is 0.
            int priority = this.E_PriorityLevel == null ? 0 : (int)this.E_PriorityLevel.GetValue();


            double transactGenerationTime;
            if (this.EntryCount == 0)
            {
                transactGenerationTime = this.Simulation.Clock + this.GetTimeIncrement(meanValue, halfRange, startDelay);
            }
            else
            {
                transactGenerationTime = this.Simulation.Clock + this.GetTimeIncrement(meanValue, halfRange);
            }

            Transaction transaction = new Transaction(this.Simulation.NumbersManager.NextFreeTransactionNo, this.Simulation.Clock)
            {
                Priority = priority,
                NextEventTime = transactGenerationTime,
                MarkTime = transactGenerationTime
            };
            transaction.SetNextOwner(this);

            return transaction;
        }

        private double GetTimeIncrement(double meanValue, double halfRange, double startDelay)
        {
            if ((meanValue - halfRange) < 0)
            {
                throw new ModelingException("ADVANCE: Negative time increment!");
            }

            double increment = (meanValue - halfRange + (this.rand.NextDouble() * halfRange * 2)) + startDelay;

            return increment;
        }

        private double GetTimeIncrement(double meanValue, double halfRange)
        {
            return this.GetTimeIncrement(meanValue, halfRange, 0);
        }

        public override void Action()
        {
            Transaction transaction = this.Simulation.ActiveTransaction;
            this.EntryCount++;

            Console.WriteLine("Generated\tTime: " + this.Simulation.Clock + transaction);
            transaction.ChangeOwner(this);
            this.NextSequentialBlock.PassTransaction(transaction);
            this.Simulation.CurrentEventChain.AddAhead(transaction);

            if (this.D_CreationLimit == null || this.EntryCount < this.D_CreationLimit.GetValue())
            {
                Transaction nextTransaction = this.GenerateNext();
                this.Simulation.FutureEventChain.Add(nextTransaction);
            }
        }

        public void GenerateFirst()
        {
            Transaction transaction = this.GenerateNext();
            this.Simulation.FutureEventChain.Add(transaction);
        }
    }
}
