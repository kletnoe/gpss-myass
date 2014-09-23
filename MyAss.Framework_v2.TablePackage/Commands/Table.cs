using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2.Commands;
using MyAss.Framework_v2.OperandTypes;
using MyAss.Framework_v2.TablePackage.Entities;

namespace MyAss.Framework_v2.TablePackage.Commands
{
    public class Table : AbstractQueuedCommand
    {
        public IDoubleOperand A_TableArgument { get; private set; }
        public IDoubleOperand B_UpperLimitOfFirstClass { get; private set; }
        public IDoubleOperand C_SizeOfClasses { get; private set; }
        public IDoubleOperand D_NumberOfClasses { get; private set; }

        public Table(IDoubleOperand tableArgument, IDoubleOperand upperLimitOfFirstClass,
            IDoubleOperand sizeOfClasses, IDoubleOperand numberOfClasses)
        {
            this.A_TableArgument = tableArgument;
            this.B_UpperLimitOfFirstClass = upperLimitOfFirstClass;
            this.C_SizeOfClasses = sizeOfClasses;
            this.D_NumberOfClasses = numberOfClasses;
        }

        public override void Execute(Simulation simulation)
        {
            // A: Required. Optional only for ANOVA.
            if (this.A_TableArgument == null)
            {
                throw new ModelingException("TABLE: Operand A is required operand!");
            }

            // B: Required.
            if (this.B_UpperLimitOfFirstClass == null)
            {
                throw new ModelingException("TABLE: Operand B is required operand!");
            }
            double upperLimitOfFirstClass = B_UpperLimitOfFirstClass.GetValue();

            // C: Required.
            if (this.C_SizeOfClasses == null)
            {
                throw new ModelingException("TABLE: Operand C is required operand!");
            }
            double sizeOfClasses = C_SizeOfClasses.GetValue();

            // D: Required. Must be PosInteger.
            if (this.D_NumberOfClasses == null)
            {
                throw new ModelingException("TABLE: Operand D is required operand!");
            }
            int numberOfClasses = (int)D_NumberOfClasses.GetValue();
            if(numberOfClasses <= 0)
            {
                throw new ModelingException("TABLE: Operand D must be PosInteger!");
            }


            TableEntity tableEntity = new TableEntity(simulation, this.Id, this.A_TableArgument,
                upperLimitOfFirstClass, sizeOfClasses, numberOfClasses);
            simulation.Entities.Add(this.Id, tableEntity);

        }
    }
}
