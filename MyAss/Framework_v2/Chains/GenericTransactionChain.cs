using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MyAss.Framework_v2.Chains
{
    public class GenericTransactionChain : IEnumerable<Transaction>
    {
        private LinkedList<Transaction> baseList;
        private Func<Transaction, IComparable> orderingRef;

        public GenericTransactionChain(Func<Transaction, IComparable> orderingRef)
        {
            this.baseList = new LinkedList<Transaction>();
            this.orderingRef = orderingRef;
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

        public void AddBehind(Transaction transaction)
        {
            if (this.baseList.Count == 0)
            {
                this.baseList.AddLast(transaction);
            }
            else
            {
                var peer = this.baseList.Last;

                //while (peer.Value.Priority < transaction.Priority)
                while (this.orderingRef(peer.Value).CompareTo(this.orderingRef(transaction)) < 0)
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

        public void AddAhead(Transaction transaction)
        {
            if (this.baseList.Count == 0)
            {
                this.baseList.AddLast(transaction);
            }
            else
            {
                var peer = this.baseList.First;

                while (this.orderingRef(peer.Value).CompareTo(this.orderingRef(transaction)) > 0)
                {
                    if (peer.Next == null)
                    {
                        this.baseList.AddLast(transaction);
                        return;
                    }

                    peer = peer.Next;
                }

                this.baseList.AddBefore(peer, transaction);
            }
        }

        public IEnumerator<Transaction> GetEnumerator()
        {
            return this.baseList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.baseList.GetEnumerator();
        }
    }
}
