using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework_v2
{
    public class BiDictionary<TFirst,TSecond> : IEnumerable<KeyValuePair<TFirst, TSecond>>
    {
        private Dictionary<TFirst,TSecond> firstToSecond;
        private Dictionary<TSecond,TFirst> secondToFirst;

        public BiDictionary()
        {
            firstToSecond = new Dictionary<TFirst, TSecond>();
            secondToFirst = new Dictionary<TSecond, TFirst>();
        }

        public void Add(TFirst first, TSecond second)
        {
            if (this.firstToSecond.ContainsKey(first) ||
                this.secondToFirst.ContainsKey(second))
            {
                throw new ArgumentException("Duplicate first or second");
            }
            this.firstToSecond.Add(first, second);
            this.secondToFirst.Add(second, first);
        }

        public void ReplaceByFirst(TFirst first, TSecond second)
        {
            if (this.firstToSecond.ContainsKey(first))
            {
                TSecond currentSecond = this.firstToSecond[first];

                this.firstToSecond.Remove(first);
                this.secondToFirst.Remove(currentSecond);
            }

            this.firstToSecond.Add(first, second);
            this.secondToFirst.Add(second, first);
        }

        public void ReplaceBySecond(TFirst first, TSecond second)
        {
            if (this.secondToFirst.ContainsKey(second))
            {
                TFirst currentFirst = this.secondToFirst[second];

                this.firstToSecond.Remove(currentFirst);
                this.secondToFirst.Remove(second);
            }

            this.firstToSecond.Add(first, second);
            this.secondToFirst.Add(second, first);
        }

        public TSecond GetByFirst(TFirst first)
        {
            return this.firstToSecond[first];
        }

        public TFirst GetBySecond(TSecond second)
        {
            return this.secondToFirst[second];
        }

        public bool ContainsByFirst(TFirst first)
        {
            return this.firstToSecond.ContainsKey(first);
        }

        public bool ContainsBySecond(TSecond second)
        {
            return this.secondToFirst.ContainsKey(second);
        }

        public bool RemoveByFirst(TFirst first)
        {
            if (this.firstToSecond.ContainsKey(first))
            {
                TSecond second = this.firstToSecond[first];

                this.firstToSecond.Remove(first);
                this.secondToFirst.Remove(second);

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveBySecond(TSecond second)
        {
            if (this.secondToFirst.ContainsKey(second))
            {
                TFirst first = this.secondToFirst[second];

                this.firstToSecond.Remove(first);
                this.secondToFirst.Remove(second);

                return true;
            }
            else
            {
                return false;
            }
        }

        public void Clear()
        {
            this.firstToSecond.Clear();
            this.secondToFirst.Clear();
        }

        public IEnumerator<KeyValuePair<TFirst, TSecond>> GetEnumerator()
        {
            foreach (var item in this.firstToSecond)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
