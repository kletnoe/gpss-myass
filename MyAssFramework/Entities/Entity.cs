using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.Entities
{
    public class Entity : IEntity
    {
        public int Id { get; protected set; }
    }
}
