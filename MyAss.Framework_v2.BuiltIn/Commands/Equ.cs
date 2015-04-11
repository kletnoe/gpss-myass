using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2.Commands;
using MyAss.Framework_v2.OperandTypes;

namespace MyAss.Framework_v2.BuiltIn.Commands
{
    public class Equ : AnyQueuedCommand
    {
        public IDoubleOperand X_Expression { get; private set; }

        public Equ(IDoubleOperand expression)
        {
            this.X_Expression = expression;
        }

        public override void Execute(Simulation simulation)
        {
            // A: Required. The operand must be Expression.
            if (this.X_Expression == null)
            {
                throw new ModelingException("EQU: Operand X must be Expression!");
            }
            double value = this.X_Expression.GetValue();

            string name = simulation.NamesAndVarsDictionary.GetNameByValue(this.Id);
            ReferencedNumber refNo = simulation.NamesAndVarsDictionary.GetValue(name);
            simulation.NamesAndVarsDictionary.Remove(name);
            refNo.SetValueDouble(value);
            simulation.NamesAndVarsDictionary.AddVar(name, refNo);

            // Equ command Id is useless.
            this.Id = 0;
        }
    }
}
