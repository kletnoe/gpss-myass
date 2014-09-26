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
        protected Simulation simulation;

        private NamesAndVarsDictionary namesAndVarsDictionary;
        private List<IBlock> blocks;
        private List<ICommand> commands;

        public NamesAndVarsDictionary NamesDictionary
        {
            get
            {
                return this.namesAndVarsDictionary;
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
            this.namesAndVarsDictionary = new NamesAndVarsDictionary();
            this.blocks = new List<IBlock>();
            this.commands = new List<ICommand>();
        }

        public void SetSimulation(Simulation simulation)
        {
            this.simulation = simulation;
        }

        public Simulation GetSimulation()
        {
            return this.simulation;
        }

        public void AddName(string name, int nameId)
        {
            this.namesAndVarsDictionary.AddName(name, nameId);
        }

        public void ReplaceNameId(string name, int nameId)
        {
            if (this.namesAndVarsDictionary.ContainsKey(name))
            {
                this.namesAndVarsDictionary.Remove(name);
                this.namesAndVarsDictionary.AddName(name, nameId);
            }
        }

        public void AddVerb(IBlock block)
        {
            this.blocks.Add(block);
        }

        public void AddVerb(ICommand command)
        {
            this.commands.Add(command);
        }
    }
}
