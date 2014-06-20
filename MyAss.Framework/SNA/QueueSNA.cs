using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework.Entities;

namespace MyAssFramework.SNA
{
    public static partial class SNA
    {
        public static int Q(int entityId)
        {
            IEntity entity = Simulation.It.GetEntity(entityId);

            if (entity is QueueEntity)
            {
                return ((QueueEntity)entity).CurrentContent;
            }
            else
            {
                throw new ModelingException("SNA::Q: Entity is not a Queue. EntityId: " + entityId);
            }
        }

        public static int Q(string entityName)
        {
            return SNA.Q(Simulation.It.Names[entityName]);
        }

        public static double QA(int entityId)
        {
            IEntity entity = Simulation.It.GetEntity(entityId);

            if (entity is QueueEntity)
            {
                return ((QueueEntity)entity).AverageContent;
            }
            else
            {
                throw new ModelingException("SNA::QA: Entity is not a Queue. EntityId: " + entityId);
            }
        }

        public static double QC(int entityId)
        {
            IEntity entity = Simulation.It.GetEntity(entityId);

            if (entity is QueueEntity)
            {
                return ((QueueEntity)entity).EntriesCount;
            }
            else
            {
                throw new ModelingException("SNA::QC: Entity is not a Queue. EntityId: " + entityId);
            }
        }

        public static double QM(int entityId)
        {
            IEntity entity = Simulation.It.GetEntity(entityId);

            if (entity is QueueEntity)
            {
                return ((QueueEntity)entity).MaxContent;
            }
            else
            {
                throw new ModelingException("SNA::QM: Entity is not a Queue. EntityId: " + entityId);
            }
        }

        public static double QT(int entityId)
        {
            IEntity entity = Simulation.It.GetEntity(entityId);

            if (entity is QueueEntity)
            {
                return ((QueueEntity)entity).AverageResidenceTime;
            }
            else
            {
                throw new ModelingException("SNA::QT: Entity is not a Queue. EntityId: " + entityId);
            }
        }

        public static double QX(int entityId)
        {
            IEntity entity = Simulation.It.GetEntity(entityId);

            if (entity is QueueEntity)
            {
                return ((QueueEntity)entity).AverageResidenceTimeNonZero;
            }
            else
            {
                throw new ModelingException("SNA::QX: Entity is not a Queue. EntityId: " + entityId);
            }
        }

        public static double QZ(int entityId)
        {
            IEntity entity = Simulation.It.GetEntity(entityId);

            if (entity is QueueEntity)
            {
                return ((QueueEntity)entity).ZeroEntriesCount;
            }
            else
            {
                throw new ModelingException("SNA::QZ: Entity is not a Queue. EntityId: " + entityId);
            }
        }
    }
}
