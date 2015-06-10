using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.OperandTypes;

namespace MyAss.Framework_v2.BuiltIn.Blocks
{
    public class Adopt : AnyBlock
    {
        public IDoubleOperand A_AssemblySet { get; private set; }

        public Adopt(IDoubleOperand assemblySet)
        {
            this.A_AssemblySet = assemblySet;
        }

        public override void Action()
        {
            // A: Required. The operand must be PosInteger.
            if (this.A_AssemblySet == null)
            {
                throw new ModelingException("ADOPT: Operand A must be PosInteger!");
            }
            int assemblySet = (int)this.A_AssemblySet.GetValue();
            if (assemblySet <= 0)
            {
                throw new ModelingException("ADOPT: Operand A must be PosInteger!");
            }


            Transaction transaction = this.Simulation.ActiveTransaction;
            this.EntryCount++;

            transaction.AssemblySet = assemblySet;

            Console.WriteLine("Adopted\tTime: " + this.Simulation.Clock + transaction, ConsoleColor.Gray);
            transaction.ChangeOwner(this);
            this.NextSequentialBlock.PassTransaction(transaction);
            this.Simulation.CurrentEventChain.AddAhead(transaction);
        }
    }
}
