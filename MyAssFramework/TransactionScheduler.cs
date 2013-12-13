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

                if (Simulation.CurrentEventChain.Count > 0)
                {
                    Transaction transaction = Simulation.CurrentEventChain.First;
                    Simulation.CurrentEventChain.RemoveFirst();
                    Simulation.ActiveTransction = transaction;
                    transaction.Owner.Action();
                }
                else
                {
                    UpdateTime();
                }


            } while (Simulation.TerminationsCount > 0 
                //|| Simulation.CurrentEventChain.Count > 0 // temporary for compatibility test
                );

            System.Console.WriteLine("End");
        }

        static public void UpdateTime()
        {
            double nextTime = Simulation.FutureEventChain.First.NextEventTime;

            while (Simulation.FutureEventChain.Count != 0 && Simulation.FutureEventChain.First.NextEventTime == nextTime)
            {
                Transaction transaction = Simulation.FutureEventChain.First;
                Simulation.FutureEventChain.RemoveFirst();
                Simulation.CurrentEventChain.AddBehind(transaction);
            }

            Simulation.Clock = nextTime;
        }
    }
}
