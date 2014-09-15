using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.Commands;
using MyAss.Framework_v2.OperandTypes;

namespace MyAss.Framework_v2.BuiltIn.Commands
{
    public class Start : AbstractQueuedCommand
    {
        public IDoubleOperand A_TerminationCount { get; private set; }
        public LiteralOperand B_PrintoutOperand { get; private set; }
        public object C_NotInUse { get; private set; }
        public IDoubleOperand D_ChainPrintout { get; private set; }

        public Start(IDoubleOperand terminationCount, LiteralOperand printoutOperand, object notInUse,
            IDoubleOperand chainPrintout)
        {
            this.A_TerminationCount = terminationCount;
            this.B_PrintoutOperand = printoutOperand;
            this.C_NotInUse = notInUse;
            this.D_ChainPrintout = chainPrintout;
        }


        public override void Execute(Simulation simulation)
        {
            // Required. The operand must be PosInteger.
            int terminationCount = this.A_TerminationCount == null ? -1 : (int)this.A_TerminationCount.GetValue();
            if (terminationCount < 0)
            {
                throw new ModelingException("STORAGE: Operand A must be PosInteger!");
            }

            // Nullable Literal.
            string relationalOp = this.B_PrintoutOperand == null ? null : this.B_PrintoutOperand.Value;

            // Not Required. The operand must be PosInteger.
            int chainPrintout = this.D_ChainPrintout == null ? 0 : (int)this.D_ChainPrintout.GetValue();
            if (terminationCount < 0)
            {
                throw new ModelingException("STORAGE: Operand D must be PosInteger!");
            }

            simulation.TerminationsCount = terminationCount;

            foreach (var block in simulation.Blocks)
            {
                if (block.Value is ITransactionGeneratableBlock)
                {
                    (block.Value as ITransactionGeneratableBlock).GenerateFirst(simulation);
                }
            }
        }
    }
}
