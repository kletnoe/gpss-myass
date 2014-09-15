using System;
using System.Collections.Generic;
//using System.Linq;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.Chains;
using MyAss.Framework_v2.Commands;
using MyAss.Framework_v2.Entities;

namespace MyAss.Framework_v2
{
    public sealed class Simulation
    {
        public SimulationNumbersManager NumbersManager { get; private set; }
        public PriorityTransactionChain CurrentEventChain { get; private set; }
        public FutureTransactionChain FutureEventChain { get; private set; }
        public Queue<ICommand> CommandQueue { get; private set; }

        public BiDictionary<int, string> NamesDictionary { get; private set; }
        public Dictionary<int, IBlock> Blocks { get; private set; }
        public Dictionary<int, IEntity> Entities { get; private set; }

        public double Clock { get; set; }
        public int TerminationsCount { get; set; }
        public Transaction ActiveTransaction { get; set; }

        public bool IsReportNeeded { get; set; }

        private Simulation()
        {
            this.NumbersManager = new SimulationNumbersManager();
            this.CurrentEventChain = new PriorityTransactionChain();
            this.FutureEventChain = new FutureTransactionChain();
            this.CommandQueue = new Queue<ICommand>();
            this.Blocks = new Dictionary<int, IBlock>();
            this.Entities = new Dictionary<int, IEntity>();
        }

        public Simulation(AbstractModel model)
            : this()
        {
            model.Simulation = this;

            this.ProcessNames(model);
            this.ProcessBlocks(model);
            this.ProcessCommands(model);

            TransactionScheduler scheduler = new TransactionScheduler(this);
            scheduler.MainLoop();

            if (this.IsReportNeeded)
            {
                //StandardReport.PrintReport(this);
            }
        }

        private void ProcessNames(AbstractModel model)
        {
            this.NamesDictionary = model.NamesDictionary;
        }

        private void ProcessBlocks(AbstractModel model)
        {
            int i = 1;
            foreach (var block in model.Blocks)
            {
                block.SetId(i);
                this.Blocks.Add(i, block);

                // Set NSB for previous
                if (i > 1)
                {
                    this.Blocks[i - 1].NextSequentialBlock = block;
                }

                i++;
            }
        }

        private void ProcessCommands(AbstractModel model)
        {
            foreach (ICommand command in model.Commands)
            {
                if (command is AbstractQueuedCommand)
                {
                    this.CommandQueue.Enqueue(command);
                }
                else
                {
                    command.Execute(this);
                }
            }

            while (this.CommandQueue.Count > 0)
            {
                ICommand command = this.CommandQueue.Dequeue();
                command.Execute(this);
            }
        }


        // TODO: Try to remove this
        public IEntity GetEntity(int id)
        {
            return this.Entities[id];
        }
    }
}
