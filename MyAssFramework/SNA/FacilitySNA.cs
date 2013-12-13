using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework.Entities;
using MyAssFramework.Resources;

namespace MyAssFramework.SNA
{
    public static partial class SNA
    {
        public static object F(int entityId)
        {
            IEntity entity = Simulation.GetEntity(entityId);

            if (entity is FacilityEntity)
            {
                return ((FacilityEntity)entity).IsBuisy;
            }
            else
            {
                throw new ModelingException(String.Format(ExceptionMessages.WrongSnaEntity, "F", "Facility", entityId));
            }
        }

        public static object FC(int entityId)
        {
            IEntity entity = Simulation.GetEntity(entityId);

            if (entity is FacilityEntity)
            {
                return ((FacilityEntity)entity).CaptureCount;
            }
            else
            {
                throw new ModelingException(String.Format(ExceptionMessages.WrongSnaEntity, "FC", "Facility", entityId));
            }
        }

        public static object FI(int entityId)
        {
            IEntity entity = Simulation.GetEntity(entityId);

            if (entity is FacilityEntity)
            {
                return ((FacilityEntity)entity).IsInterrupted;
            }
            else
            {
                throw new ModelingException(String.Format(ExceptionMessages.WrongSnaEntity, "FI", "Facility", entityId));
            }
        }

        public static object FR(int entityId)
        {
            IEntity entity = Simulation.GetEntity(entityId);

            if (entity is FacilityEntity)
            {
                return ((FacilityEntity)entity).Utilization;
            }
            else
            {
                throw new ModelingException(String.Format(ExceptionMessages.WrongSnaEntity, "FR", "Facility", entityId));
            }
        }

        public static object FT(int entityId)
        {
            IEntity entity = Simulation.GetEntity(entityId);

            if (entity is FacilityEntity)
            {
                return ((FacilityEntity)entity).AverageHoldingTime;
            }
            else
            {
                throw new ModelingException(String.Format(ExceptionMessages.WrongSnaEntity, "FT", "Facility", entityId));
            }
        }

        public static object FV(int entityId)
        {
            IEntity entity = Simulation.GetEntity(entityId);

            if (entity is FacilityEntity)
            {
                return ((FacilityEntity)entity).IsAvaliable;
            }
            else
            {
                throw new ModelingException(String.Format(ExceptionMessages.WrongSnaEntity, "FV", "Facility", entityId));
            }
        }
    }
}
