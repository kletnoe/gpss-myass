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
        private static int transactionNo = 1;
        private static int entityNo = 10000;
        private static int blockNo = 1;

        public static double Clock { get; set; }
        public static int TerminationsCount { get; set; }

        public static IList<IEntity> Entities { get; set; }
        public static IList<IBlock> Blocks { get; set; }

        public static Transaction ActiveTransction { get; set; }

        public static PriorityTransactionChain CurrentEventChain = new PriorityTransactionChain();
        public static FutureTransactionChain FutureEventChain = new FutureTransactionChain();
        public static Queue<ICommand> CommandQueue = new Queue<ICommand>();

        public static int NextTransactionNo()
        {
            int value = transactionNo;
            Simulation.transactionNo++;
            return value;
        }

        public static void ResetTransactionCounter()
        {
            Simulation.transactionNo = 1;
        }

        public static int NextEntityNo()
        {
            int value = entityNo;
            Simulation.entityNo++;
            return value;
        }

        public static IEntity GetEntity(int id)
        {
            return Simulation.Entities.Where(x => x.Id == id).FirstOrDefault();
        }

        public static int NextBlockNo()
        {
            int value = blockNo;
            Simulation.blockNo++;
            return value;
        }

        public static IBlock GetBlock(int id)
        {
            return Simulation.Blocks.Where(x => x.Id == id).First();
        }

        public static void Start(int termonationsCount)
        {
            Simulation.ResolveNSB();
            Simulation.TerminationsCount = termonationsCount;
            TransactionScheduler.MainLoop();
        }

        private static void ResolveNSB()
        {
            for (int i = 0; i < Simulation.Blocks.Count; i++)
            {
                IBlock block = Simulation.Blocks.ElementAt(i);

                if (block.GetType() != typeof(Terminate))
                {
                    block.NextSequentialBlock = Simulation.Blocks.ElementAt(i + 1);
                }
            }
        }
    }
}
