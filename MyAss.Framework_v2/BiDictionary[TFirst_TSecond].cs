using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework_v2
{
    public class BiDictionary<TFirst,TSecond>
    {
        private Dictionary<TFirst,TSecond> firstToSecond = new Dictionary<TFirst, TSecond>();
        private Dictionary<TSecond,TFirst> secondToFirst = new Dictionary<TSecond, TFirst>();

        public IReadOnlyDictionary<TFirst,TSecond> FirstToSecond
        {
            get
            {
                return this.firstToSecond;
            }
        }

        public IReadOnlyDictionary<TSecond,TFirst> SecondToFirst
        {
            get
            {
                return this.secondToFirst;
            }
        }

        public void Add(TFirst first, TSecond second)
        {
            if (this.firstToSecond.ContainsKey(first) ||
                this.secondToFirst.ContainsKey(second))
            {
                throw new ArgumentException("Duplicate first or second");
            }
            this.firstToSecond.Add(first, second);
            this. secondToFirst.Add(second, first);
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

        public void Clear()
        {
            this.firstToSecond.Clear();
            this.secondToFirst.Clear();
        }
    }
}
