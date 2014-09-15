using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.Commands;

namespace MyAss.Framework_v2
{
    public abstract class AbstractModel
    {
        public Simulation Simulation { get; set; }

        private Dictionary<string, int> names;
        private List<IBlock> blocks;
        private List<ICommand> commands;

        public IReadOnlyDictionary<string, int> Names
        {
            get
            {
                return this.names;
            }
        }

        public IReadOnlyList<IBlock> Blocks
        {
            get
            {
                return this.blocks;
            }
        }

        public IReadOnlyList<ICommand> Commands
        {
            get
            {
                return this.commands;
            }
        }

        public AbstractModel()
        {
            this.names = new Dictionary<string, int>();
            this.blocks = new List<IBlock>();
            this.commands = new List<ICommand>();
        }

        public void AddName(string key, int value)
        {
            this.names.Add(key, value);
        }

        public void AddVerb(IBlock block)
        {
            this.blocks.Add(block);
        }

        public void AddVerb(ICommand command)
        {
            this.commands.Add(command);
        }

        public abstract AbstractModel Construct();
    }
}
