using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework.Entities;

namespace MyAssFramework.SNA
{
    public static partial class SNA
    {
        public static int R(int entityId)
        {
            IEntity entity = Simulation.GetEntity(entityId);

            if (entity is StorageEntity)
            {
                return ((StorageEntity)entity).RemainingCapacity;
            }
            else
            {
                throw new ModelingException("SNA::R: Entity is not a Storage. EntityId: " + entityId);
            }
        }

        public static int S(int entityId)
        {
            IEntity entity = Simulation.GetEntity(entityId);

            if (entity is StorageEntity)
            {
                return ((StorageEntity)entity).CurrentCount;
            }
            else
            {
                throw new ModelingException("SNA::S: Entity is not a Storage. EntityId: " + entityId);
            }
        }

        public static double SA(int entityId)
        {
            IEntity entity = Simulation.GetEntity(entityId);

            if (entity is StorageEntity)
            {
                return ((StorageEntity)entity).AverageContent;
            }
            else
            {
                throw new ModelingException("SNA::SA: Entity is not a Storage. EntityId: " + entityId);
            }
        }

        public static int SC(int entityId)
        {
            IEntity entity = Simulation.GetEntity(entityId);

            if (entity is StorageEntity)
            {
                return ((StorageEntity)entity).EntriesCount;
            }
            else
            {
                throw new ModelingException("SNA::SC: Entity is not a Storage. EntityId: " + entityId);
            }
        }

        public static int SM(int entityId)
        {
            IEntity entity = Simulation.GetEntity(entityId);

            if (entity is StorageEntity)
            {
                return ((StorageEntity)entity).MaxContent;
            }
            else
            {
                throw new ModelingException("SNA::SM: Entity is not a Storage. EntityId: " + entityId);
            }
        }

        public static double SR(int entityId)
        {
            IEntity entity = Simulation.GetEntity(entityId);

            if (entity is StorageEntity)
            {
                return ((StorageEntity)entity).Utilization;
            }
            else
            {
                throw new ModelingException("SNA::SR: Entity is not a Storage. EntityId: " + entityId);
            }
        }

        public static double ST(int entityId)
        {
            IEntity entity = Simulation.GetEntity(entityId);

            if (entity is StorageEntity)
            {
                return ((StorageEntity)entity).AverageHoldingTime;
            }
            else
            {
                throw new ModelingException("SNA::ST: Entity is not a Storage. EntityId: " + entityId);
            }
        }

        public static int SE(int entityId)
        {
            IEntity entity = Simulation.GetEntity(entityId);

            if (entity is StorageEntity)
            {
                return ((StorageEntity)entity).IsEmpty ? 1 : 0;
            }
            else
            {
                throw new ModelingException("SNA::SE: Entity is not a Storage. EntityId: " + entityId);
            }
        }

        public static int SF(int entityId)
        {
            IEntity entity = Simulation.GetEntity(entityId);

            if (entity is StorageEntity)
            {
                return ((StorageEntity)entity).IsFull ? 1 : 0;
            }
            else
            {
                throw new ModelingException("SNA::SF: Entity is not a Storage. EntityId: " + entityId);
            }
        }

        public static int SV(int entityId)
        {
            IEntity entity = Simulation.GetEntity(entityId);

            if (entity is StorageEntity)
            {
                return ((StorageEntity)entity).IsAvaliable ? 1 : 0;
            }
            else
            {
                throw new ModelingException("SNA::SV: Entity is not a Storage. EntityId: " + entityId);
            }
        }
    }
}
