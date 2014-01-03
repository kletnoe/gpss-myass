using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework.Commands;
using MyAssFramework.Entities;
using MyAssFramework.Blocks;
using MyAssFramework.Chains;

namespace MyAssFramework
{
    public class Simulation
    {
        private static Simulation instance;

        private int transactionNo = 1;
        private int entityNo = 10000;
        private int blockNo = 1;

        public double Clock { get; set; }
        public int TerminationsCount { get; set; }


        public IDictionary<string, int> Names { get; set; }
        public IList<IEntity> Entities { get; set; }
        public IList<IBlock> Blocks { get; set; }

        public Transaction ActiveTransction { get; set; }

        public PriorityTransactionChain CurrentEventChain = new PriorityTransactionChain();
        public FutureTransactionChain FutureEventChain = new FutureTransactionChain();
        public Queue<ICommand> CommandQueue = new Queue<ICommand>();

        private Simulation() {}

        public static Simulation It
        {
            get
            {
                if (Simulation.instance == null)
                {
                    Simulation.instance = new Simulation();
                }
                return Simulation.instance;
            }
        }


        public int NextTransactionNo()
        {
            int value = this.transactionNo;
            this.transactionNo++;
            return value;
        }

        public void ResetTransactionCounter()
        {
            this.transactionNo = 1;
        }

        public int NextEntityNo()
        {
            int value = this.entityNo;
            this.entityNo++;
            return value;
        }

        public IEntity GetEntity(int id)
        {
            return this.Entities.Where(x => x.Id == id).FirstOrDefault();
        }

        public int NextBlockNo()
        {
            int value = this.blockNo;
            this.blockNo++;
            return value;
        }

        public IBlock GetBlock(int id)
        {
            return this.Blocks.Where(x => x.Id == id).First();
        }

        public void Start(int termonationsCount)
        {
            this.ResolveNSB();
            this.TerminationsCount = termonationsCount;
            TransactionScheduler.MainLoop();
        }

        private void ResolveNSB()
        {
            for (int i = 0; i < this.Blocks.Count; i++)
            {
                IBlock block = this.Blocks.ElementAt(i);

                if (block.GetType() != typeof(Terminate))
                {
                    block.NextSequentialBlock = this.Blocks.ElementAt(i + 1);
                }
            }
        }
    }
}
