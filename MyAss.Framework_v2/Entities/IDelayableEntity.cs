﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework_v2.Entities
{
    public interface IDelayableEntity
    {
        LinkedList<Transaction> DelayChain { get; }
    }
}