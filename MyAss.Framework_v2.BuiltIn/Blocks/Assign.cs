using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.OperandTypes;

namespace MyAss.Framework_v2.BuiltIn.Blocks
{
    public class Assign : AnyBlock
    {
        public IDoubleOperand A_ParameterId { get; private set; }
        public IDoubleOperand B_Value { get; private set; }
        public IDoubleOperand C_FunctionNumber { get; private set; }

        public Assign(IDoubleOperand parameterId, IDoubleOperand value, IDoubleOperand functionNumber)
        {
            this.A_ParameterId = parameterId;
            this.B_Value = value;
            this.C_FunctionNumber = functionNumber;
        }

        // TODO: Operand c "function modifier".
        public override void Action(Simulation simulation)
        {
            // A: Required. The operand must be PosInteger.
            if (A_ParameterId == null)
            {
                throw new ModelingException("ASSIGN: Operand A is required operand!");
            }
            int parameterId = (int)this.A_ParameterId.GetValue();
            if (parameterId <= 0)
            {
                throw new ModelingException("ASSIGN: Operand A must be PosInteger!");
            }

            // B: Required.
            if (B_Value == null)
            {
                throw new ModelingException("ASSIGN: Operand B is required operand!");
            }
            double value = this.B_Value.GetValue();

            // C: Optional



            Transaction transaction = simulation.ActiveTransaction;
            this.EntryCount++;

            transaction.TransactionParameters.Add(parameterId, value);

            Console.WriteLine("Assigned\tTime: " + simulation.Clock + transaction, ConsoleColor.Gray);
            transaction.ChangeOwner(simulation, this);
            this.NextSequentialBlock.PassTransaction(transaction);
            simulation.CurrentEventChain.AddAhead(transaction);
        }
    }
}
