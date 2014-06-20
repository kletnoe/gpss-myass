using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework.Entities
{
    public class TableEntity : AbstractEntity
    {
        public TableEntity(int id)
        {
            this.Id = id;
        }

        public override void UpdateStats()
        {

        }
    }
}
