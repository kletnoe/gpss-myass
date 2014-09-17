﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework_v2.Commands
{
    public abstract class AbstractImmediateCommand : ICommand
    {
        public int Id { get; protected set; }
        public string Label { get; protected set; }

        public abstract void Execute(Simulation simulation);

        public void SetId(int id)
        {
            this.Id = id;
        }
    }
}