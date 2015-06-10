using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2.Commands;

namespace MyAss.Framework_v2
{
    internal sealed class TransactionScheduler
    {
        private Simulation simulation;

        public TransactionScheduler(Simulation simulation)
        {
            this.simulation = simulation;
        }

        public void MainLoop()
        {
            //int step = 1000;

            do
            {
                this.ExecuteCommands();

                if (this.simulation.CurrentEventChain.Count == 0)
                {
                    if (this.simulation.FutureEventChain.Count == 0)
                    {
                        throw new ModelingException("Future Event Chain is empty!");
                    }
                    else
                    {
                        UpdateTime();
                    }
                }

                Transaction transaction = this.simulation.CurrentEventChain.First;
                this.simulation.CurrentEventChain.RemoveFirst();
                this.simulation.ActiveTransaction = transaction;
                transaction.NextOwner.Action();

                //if(step < this.simulation.Clock)
                //{
                //    StandardReport.PrintReport(simulation);
                //    System.Console.WriteLine(step);
                //    step += 1000;
                //}


            } while (this.simulation.TerminationsCount > 0
                || this.simulation.CurrentEventChain.Count > 0 // temporary for compatibility test
                );

            foreach (var entity in this.simulation.Entities)
            {
                entity.Value.UpdateStats();
            }

            System.Console.WriteLine("End");
        }

        private void ExecuteCommands()
        {
            while (this.simulation.CommandQueue.Count > 0)
            {
                AnyCommand command = this.simulation.CommandQueue.Dequeue();
                command.Execute(this.simulation);
            }
        }

        private void UpdateTime()
        {
            double nextTime = this.simulation.FutureEventChain.First.NextEventTime;

            while (this.simulation.FutureEventChain.Count != 0 && this.simulation.FutureEventChain.First.NextEventTime == nextTime)
            {
                Transaction transaction = this.simulation.FutureEventChain.First;
                this.simulation.FutureEventChain.RemoveFirst();
                this.simulation.CurrentEventChain.AddBehind(transaction);
            }

            this.simulation.Clock = nextTime;
        }
    }
}
