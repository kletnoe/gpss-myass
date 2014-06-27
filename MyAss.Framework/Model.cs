using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework.Blocks;
using MyAss.Framework.Commands;

namespace MyAss.Framework
{
    public class Model
    {
        private IList<IBlock> blocks;
        private IList<ICommand> commands;

        public Model()
        {
            this.blocks = new List<IBlock>();
            this.commands = new List<ICommand>();
        }

        public void Add(IBlock block)
        {
            this.blocks.Add(block);
        }

        public void Add(ICommand command)
        {
            this.commands.Add(command);
        }
    }
}
