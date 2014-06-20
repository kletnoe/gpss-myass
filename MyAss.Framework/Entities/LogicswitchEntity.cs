using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework.Entities
{
    public class LogicswitchEntity : AbstractEntity
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

        public override void UpdateStats()
        {

        }
    }
}
