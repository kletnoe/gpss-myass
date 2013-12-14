using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework.Entities;

namespace MyAssFramework.SNA
{
    public static partial class SNA
    {
        public static SavevalueType X(int entityId)
        {
            IEntity entity = Simulation.It.GetEntity(entityId);

            if (entity is SavevalueEntity)
            {
                return ((SavevalueEntity)entity).Value;
            }
            else
            {
                throw new ModelingException("SNA::X: Entity is not a Savevalue. EntityId: " + entityId);
            }
        }
    }
}
