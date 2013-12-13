using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.Entities
{
    public class LogicswitchEntity : Entity
    {
        private bool set;

        public bool IsSet
        {
            get
            {
                return this.set;
            }
        }

        public LogicswitchEntity(int id)
        {
            this.Id = id;
            this.set = false;
        }
    }
}
