using MyAss.Framework_v2.BuiltIn.Entities;
using MyAss.Framework_v2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework_v2.BuiltIn.SNA
{
    public static class SavevalueSNA
    {
        public static double X(Simulation simulation, int entityId)
        {
            if (!simulation.Entities.ContainsKey(entityId))
            {
                return 0;
            }

            IEntity entity = simulation.GetEntity(entityId);

            if (entity is SavevalueEntity)
            {
                return (double)((SavevalueEntity)entity).Value;
            }
            else
            {
                throw new ModelingException("SNA::X: Entity is not a Savevalue. EntityId: " + entityId);
            }
        }
    }
}
