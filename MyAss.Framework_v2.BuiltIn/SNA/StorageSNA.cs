using MyAss.Framework_v2.BuiltIn.Entities;
using MyAss.Framework_v2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework_v2.BuiltIn.SNA
{
    public static class StorageSNA
    {
        public static double SF(Simulation simulation, int entityId)
        {
            if (!simulation.Entities.ContainsKey(entityId))
            {
                return 0;
            }

            AnyEntity entity = simulation.GetEntity(entityId);

            if (entity is StorageEntity)
            {
                return ((StorageEntity)entity).SF;
            }
            else
            {
                throw new ModelingException("SNA::SF: Entity is not a Storage. EntityId: " + entityId);
            }
        }
    }
}
