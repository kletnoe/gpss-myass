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

        private BiDictionary<int, string> namesDictionary;
        private List<IBlock> blocks;
        private List<ICommand> commands;

        public BiDictionary<int, string> NamesDictionary
        {
            get
            {
                return this.namesDictionary;
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
            this.namesDictionary = new BiDictionary<int, string>();
            this.blocks = new List<IBlock>();
            this.commands = new List<ICommand>();
        }

        public void AddName(int key, string value)
        {
            this.namesDictionary.Add(key, value);
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
