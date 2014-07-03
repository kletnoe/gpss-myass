using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MyAss.Framework_v2.Chains
{
    internal class FutureTransactionChain : IEnumerable<Transaction>
    {
        private LinkedList<Transaction> baseList;

        public FutureTransactionChain()
        {
            this.baseList = new LinkedList<Transaction>();
        }

        public Transaction First
        {
            get
            {
                return this.baseList.First.Value;
            }
        }

        public Transaction Last
        {
            get
            {
                return this.baseList.Last.Value;
            }
        }

        public int Count
        {
            get
            {
                return this.baseList.Count;
            }
        }

        public void RemoveFirst()
        {
            this.baseList.RemoveFirst();
        }

        public void Add(Transaction transaction)
        {
            if (this.baseList.Count == 0)
            {
                this.baseList.AddLast(transaction);
            }
            else
            {
                var peer = this.baseList.Last;

                while (peer.Value.NextEventTime > transaction.NextEventTime)
                {
                    if (peer.Previous == null)
                    {
                        this.baseList.AddFirst(transaction);
                        return;
                    }

                    peer = peer.Previous;
                }

                this.baseList.AddAfter(peer, transaction);
            }
        }

        public IEnumerator<Transaction> GetEnumerator()
        {
            return baseList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return baseList.GetEnumerator();
        }
    }
}
