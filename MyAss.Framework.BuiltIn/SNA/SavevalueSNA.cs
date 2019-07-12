using MyAss.Framework.BuiltIn.Entities;
using MyAss.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework.BuiltIn.SNA
{
    public static class SavevalueSNA
    {
        public static double X(Simulation simulation, int entityId)
        {
            if (!simulation.Entities.ContainsKey(entityId))
            {
                return 0;
            }

            AnyEntity entity = simulation.GetEntity(entityId);

            if (entity is SavevalueEntity)
            {
                return (double)((SavevalueEntity)entity).X;
            }
            else
            {
                throw new ModelingException("SNA::X: Entity is not a Savevalue. EntityId: " + entityId);
            }
        }
    }
}
