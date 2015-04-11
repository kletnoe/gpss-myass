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
        public OrderedTransactionChain FutureEventChain { get; private set; }
        public Queue<AnyCommand> CommandQueue { get; private set; }

        public NamesAndVarsDictionary NamesAndVarsDictionary { get; private set; }
        public Dictionary<int, AnyBlock> Blocks { get; private set; }
        public Dictionary<int, AnyEntity> Entities { get; private set; }

        public double Clock { get; set; }
        public int TerminationsCount { get; set; }
        public Transaction ActiveTransaction { get; set; }

        public bool IsReportNeeded { get; set; }

        private Simulation()
        {
            this.NumbersManager = new SimulationNumbersManager();
            this.CurrentEventChain = new PriorityTransactionChain();
            this.FutureEventChain = new OrderedTransactionChain();
            this.CommandQueue = new Queue<AnyCommand>();
            this.Blocks = new Dictionary<int, AnyBlock>();
            this.Entities = new Dictionary<int, AnyEntity>();
        }

        public Simulation(AbstractModel model)
            : this()
        {
            TransactionScheduler scheduler;

            try
            {

                model.SetSimulation(this);

                this.ProcessNames(model);
                this.ProcessBlocks(model);
                this.ProcessCommands(model);

                scheduler = new TransactionScheduler(this);
                scheduler.MainLoop();

                if (this.IsReportNeeded)
                {
                    //StandardReport.PrintReport(this);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                throw;
            }
        }

        private void ProcessNames(AbstractModel model)
        {
            this.NamesAndVarsDictionary = model.NamesDictionary;
        }

        private void ProcessBlocks(AbstractModel model)
        {
            int i = 1;
            foreach (var block in model.Blocks)
            {
                block.SetId(i);
                block.SetSimulation(this);

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
            foreach (AnyCommand command in model.Commands)
            {
                if (command is AnyQueuedCommand)
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
                AnyCommand command = this.CommandQueue.Dequeue();
                command.Execute(this);
            }
        }


        // TODO: Try to remove this
        public AnyEntity GetEntity(int id)
        {
            return this.Entities[id];
        }
        public bool ContainsEntity(int id)
        {
            return this.Entities.ContainsKey(id);
        }

        public AnyBlock GetBlock(int id)
        {
            return this.Blocks[id];
        }
        public bool ContainsBlock(int id)
        {
            return this.Blocks.ContainsKey(id);
        }
    }
}
