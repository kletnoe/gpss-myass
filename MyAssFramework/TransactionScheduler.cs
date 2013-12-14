using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework
{
    internal static class TransactionScheduler
    {
        static public void MainLoop()
        {
            do
            {

                if (Simulation.It.CurrentEventChain.Count > 0)
                {
                    Transaction transaction = Simulation.It.CurrentEventChain.First;
                    Simulation.It.CurrentEventChain.RemoveFirst();
                    Simulation.It.ActiveTransction = transaction;
                    transaction.Owner.Action();
                }
                else
                {
                    UpdateTime();
                }


            } while (Simulation.It.TerminationsCount > 0 
                //|| Simulation.It.CurrentEventChain.Count > 0 // temporary for compatibility test
                );

            System.Console.WriteLine("End");
        }

        static public void UpdateTime()
        {
            double nextTime = Simulation.It.FutureEventChain.First.NextEventTime;

            while (Simulation.It.FutureEventChain.Count != 0 && Simulation.It.FutureEventChain.First.NextEventTime == nextTime)
            {
                Transaction transaction = Simulation.It.FutureEventChain.First;
                Simulation.It.FutureEventChain.RemoveFirst();
                Simulation.It.CurrentEventChain.AddBehind(transaction);
            }

            Simulation.It.Clock = nextTime;
        }
    }
}
