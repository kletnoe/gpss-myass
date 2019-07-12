using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework.Blocks;
using MyAss.Framework.OperandTypes;

namespace MyAss.Framework.BuiltIn.Blocks
{
    public class Priority : AnyBlock
    {
        public IDoubleOperand A_PriorityValue { get; private set; }
        public LiteralOperand B_BufferOption { get; private set; }

        public Priority(IDoubleOperand priorityValue, LiteralOperand bufferOption)
        {
            this.A_PriorityValue = priorityValue;
            this.B_BufferOption = bufferOption;
        }

        public override void Action()
        {
            // A: Required.
            if (A_PriorityValue == null)
            {
                throw new ModelingException("PRIORITY: Operand A is required operand!");
            }
            int parameterId = (int)this.A_PriorityValue.GetValue();

            // B: Optional.
            bool placeBehind;
            if(B_BufferOption == null)
            {
                placeBehind = false;
            }
            else if(B_BufferOption.Value == "BU")
            {
                placeBehind = true;
            }
            else
            {
                throw new ModelingException("ASSIGN: Operand B invalid LiteralOperand!");
            }

            Transaction transaction = this.Simulation.ActiveTransaction;
            this.EntryCount++;

            Console.WriteLine("Prioritized\tTime: " + this.Simulation.Clock + transaction, ConsoleColor.Gray);
            transaction.ChangeOwner(this);
            this.NextSequentialBlock.PassTransaction(transaction);

            if (placeBehind)
            {
                this.Simulation.CurrentEventChain.AddBehind(transaction);
            }
            else
            {
                this.Simulation.CurrentEventChain.AddAhead(transaction);
            }
        }

    }
}
