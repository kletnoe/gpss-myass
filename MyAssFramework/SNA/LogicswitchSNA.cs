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
        public static object LS(int entityId)
        {
            IEntity entity = Simulation.It.GetEntity(entityId);

            if (entity is LogicswitchEntity)
            {
                return ((LogicswitchEntity)entity).IsSet;
            }
            else
            {
                throw new ModelingException(String.Format(ExceptionMessages.WrongSnaEntity, "LS", "Logicswitch", entityId));
            }
        }
    }
}
