using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.OperandTypes;
using MyAss.Framework_v2.TablePackage.Entities;

namespace MyAss.Framework_v2.TablePackage.Blocks
{
    public class Tabulate : AnyBlock
    {
        public IDoubleOperand A_TableEntityId { get; private set; }
        public IDoubleOperand B_WeightingFactor { get; private set; }

        public Tabulate(IDoubleOperand tableEntityId, IDoubleOperand weightingFactor)
        {
            this.A_TableEntityId = tableEntityId;
            this.B_WeightingFactor = weightingFactor;
        }

        public override void Action(Simulation simulation)
        {
            // A: Required. The operand must be PosInteger.
            if (this.A_TableEntityId == null)
            {
                throw new ModelingException("TABULATE: Operand A is required operand!");
            }
            int entityId = (int)this.A_TableEntityId.GetValue();
            if (entityId <= 0)
            {
                throw new ModelingException("TABULATE: Operand A must be PosInteger!");
            }

            // B: The default is 1. The operand must be PosInteger.
            int weightedFactor = this.B_WeightingFactor == null ? 1 : (int)this.B_WeightingFactor.GetValue();
            if (weightedFactor <= 0)
            {
                throw new ModelingException("TABULATE: Operand B must be PosInteger!");
            }


            Transaction transaction = simulation.ActiveTransaction;
            this.EntryCount++;

            TableEntity tableEntity = (TableEntity)simulation.GetEntity(entityId);
            tableEntity.Tabulate(weightedFactor);

            //Console.WriteLine("Tabulated  \tTime: " + simulation.Clock + transaction, ConsoleColor.DarkGreen);
            transaction.ChangeOwner(simulation, this);
            this.NextSequentialBlock.PassTransaction(transaction);
            simulation.CurrentEventChain.AddAhead(transaction);
        }
    }
}
