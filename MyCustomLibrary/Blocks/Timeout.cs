using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.Entities;
using MyAss.Framework_v2.OperandTypes;

namespace MyCustomLibrary.Blocks
{
    public class Timeout : AnyBlock
    {
        public IDoubleOperand A_TimeToWait { get; private set; }
        public IDoubleOperand B_DestBlockNo { get; private set; }

        public Timeout(IDoubleOperand timeToWait, IDoubleOperand destBlockNo)
        {
            this.A_TimeToWait = timeToWait;
            this.B_DestBlockNo = destBlockNo;
        }

        public override void Action(Simulation simulation)
        {
            // A: Required.
            if (this.A_TimeToWait == null)
            {
                throw new ModelingException("VOLATILE: Operand A is required operand!");
            }
            double timeToWait = this.A_TimeToWait.GetValue();
            if (timeToWait <= 0)
            {
                throw new ModelingException("VOLATILE: Negative time span!");
            }

            // B: Required. The operand must be PosInteger.
            int destBlockNo = (int)this.B_DestBlockNo.GetValue();
            if (destBlockNo <= 0)
            {
                throw new ModelingException("VOLATILE: Operand B must be PosInteger!");
            }

            Transaction transaction = simulation.ActiveTransaction;
            this.EntryCount++;

            //Console.WriteLine("Volatiled  \tTime: " + simulation.Clock + transaction, ConsoleColor.DarkYellow);

            transaction.ChangeOwner(simulation, this);

            // Pass to NSB
            NextSequentialBlock.PassTransaction(transaction);
            simulation.CurrentEventChain.AddAhead(transaction);

            // Pass Clone to Label
            Transaction transactionClone = transaction.Split(simulation.NumbersManager.NextFreeTransactionNo);
            AnyBlock consumerOnTimeEnd = simulation.Blocks[destBlockNo];
            transactionClone.NextEventTime = simulation.Clock + timeToWait;
            transactionClone.SetNextOwner(consumerOnTimeEnd);
            simulation.FutureEventChain.Add(transactionClone);

            clones.Add(transaction, transactionClone);
        }

        public Dictionary<Transaction, Transaction> clones = new Dictionary<Transaction, Transaction>();

        public override void ActionOnDisown(Simulation simulation, Transaction transaction)
        {

            if (transaction.NextOwner == this.NextSequentialBlock.Id)
            {
                Transaction clone = clones[transaction];
                this.DisplaceTransaction(simulation, clone);
                clones.Remove(clone);
            }
            else
            {
                Transaction original = clones.Where(x => x.Value == transaction).First().Key;
                this.DisplaceTransaction(simulation, original);
                clones.Remove(original);
            }
        }

        private void DisplaceTransaction(Simulation simulation, Transaction transaction)
        {
            simulation.FutureEventChain.Remove(transaction);

            var delayableEntities = simulation.Entities.Values.OfType<IDelayableEntity>();
            foreach (var entity in delayableEntities)
            {
                entity.DelayChain.Remove(transaction);
            }
        }

    }
}
