using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.OperandTypes_Test
{
    public abstract class DirectSNA : Operand
    {
        public int EntitiyId { get; protected set; }

        public abstract double GetValue();
    }

    public class SNA_Q : DirectSNA
    {
        public SNA_Q(int entityId)
        {
            this.EntitiyId = entityId;
        }

        public override double GetValue()
        {
            return SNA.SNA.Q(this.EntitiyId);
        }
    }

    public class SNA_X : DirectSNA
    {
        public SNA_X(int entityId)
        {
            this.EntitiyId = entityId;
        }

        public override double GetValue()
        {
            return (double)SNA.SNA.X(this.EntitiyId);
        }
    }
}
