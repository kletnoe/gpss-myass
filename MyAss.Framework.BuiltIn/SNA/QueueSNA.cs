using MyAss.Framework.BuiltIn.Entities;
using MyAss.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework.BuiltIn.SNA
{
    public static class QueueSNA
    {
        public static double Q(Simulation simulation, int entityId)
        {
            if (!simulation.Entities.ContainsKey(entityId))
            {
                return 0;
            }

            AnyEntity entity = simulation.GetEntity(entityId);

            if (entity is QueueEntity)
            {
                return ((QueueEntity)entity).Q;
            }
            else
            {
                throw new ModelingException("SNA::Q: Entity is not a Queue. EntityId: " + entityId);
            }
        }
    }
}
