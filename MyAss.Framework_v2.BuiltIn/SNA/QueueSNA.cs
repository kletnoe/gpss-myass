using MyAss.Framework_v2.BuiltIn.Entities;
using MyAss.Framework_v2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework_v2.BuiltIn.SNA
{
    public static class QueueSNA
    {
        public static double Q(Simulation simulation, int entityId)
        {
            if (!simulation.Entities.ContainsKey(entityId))
            {
                return 0;
            }

            IEntity entity = simulation.GetEntity(entityId);

            if (entity is QueueEntity)
            {
                return ((QueueEntity)entity).CurrentContent;
            }
            else
            {
                throw new ModelingException("SNA::Q: Entity is not a Queue. EntityId: " + entityId);
            }
        }
    }
}
