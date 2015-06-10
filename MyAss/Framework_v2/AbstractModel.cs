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
        private List<AnyBlock> blocks;
        private List<AnyCommand> commands;

        public NamesAndVarsDictionary NamesDictionary
        {
            get
            {
                return this.namesAndVarsDictionary;
            }
        }

        public IReadOnlyList<AnyBlock> Blocks
        {
            get
            {
                return this.blocks;
            }
        }

        public IReadOnlyList<AnyCommand> Commands
        {
            get
            {
                return this.commands;
            }
        }

        public AbstractModel()
        {
            this.namesAndVarsDictionary = new NamesAndVarsDictionary();
            this.blocks = new List<AnyBlock>();
            this.commands = new List<AnyCommand>();
        }

        public void SetSimulation(Simulation simulation)
        {
            this.simulation = simulation;
        }

        public Simulation GetSimulation()
        {
            return this.simulation;
        }

        protected void AddName(string name, ReferencedNumber nameId)
        {
            this.namesAndVarsDictionary.AddName(name, nameId);
        }

        protected void ReplaceNameId(string name, ReferencedNumber nameId)
        {
            if (this.namesAndVarsDictionary.ContainsKey(name))
            {
                this.namesAndVarsDictionary.Remove(name);
                this.namesAndVarsDictionary.AddName(name, nameId);
            }
        }

        protected void AddVerb(AnyBlock block)
        {
            this.blocks.Add(block);
        }

        protected void AddVerb(AnyCommand command)
        {
            this.commands.Add(command);
        }
    }
}
