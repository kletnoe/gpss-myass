using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework_v2
{
    /// <summary>
    /// This fancy structure is created for storing Names and Vars in the same place.
    /// Particularly it's a hack for storing these ugly EQU vars.
    /// I cant figure out how to implement this more adequately.
    /// Sorry.
    /// </summary>
    public class NamesAndVarsDictionary : IEnumerable<KeyValuePair<string, ReferencedNumber>>
    {
        private BiDictionary<string, ReferencedNumber> names;
        private Dictionary<string, ReferencedNumber> vars;

        public NamesAndVarsDictionary()
        {
            this.names = new BiDictionary<string, ReferencedNumber>();
            this.vars = new Dictionary<string, ReferencedNumber>();
        }

        public void AddName(string key, ReferencedNumber value)
        {
            if (this.names.ContainsByFirst(key) || this.vars.ContainsKey(key))
            {
                throw new ArgumentException("An element with the same key already exists.");
            }

            this.names.Add(key, value);
        }

        public void AddVar(string key, ReferencedNumber value)
        {
            if (names.ContainsByFirst(key) || vars.ContainsKey(key))
            {
                throw new ArgumentException("An element with the same key already exists.");
            }

            this.vars.Add(key, value);
        }

        public ReferencedNumber GetValue(string key)
        {
            if (names.ContainsByFirst(key))
            {
                return names.GetByFirst(key);
            }
            else if (vars.ContainsKey(key))
            {
                return vars[key];
            }
            else
            {
                throw new KeyNotFoundException("The property is retrieved and key does not exist in the collection.");
            }
        }

        public string GetNameByValue(int value)
        {
            if (this.names.ContainsBySecond(new ReferencedNumber(value)))
            {
                return this.names.GetBySecond(new ReferencedNumber(value));
            }
            else
            {
                throw new KeyNotFoundException("The property is retrieved and value does not exist in the collection.");
            }
        }

        public bool ContainsKey(string key)
        {
            return this.names.ContainsByFirst(key) || this.vars.ContainsKey(key);
        }

        public bool ContainsNameValue(int value)
        {
            return this.names.ContainsBySecond(new ReferencedNumber(value));
        }

        public bool Remove(string key)
        {
            // Full-Or operator
            return this.names.RemoveByFirst(key) | this.vars.Remove(key);
        }

        public IEnumerator<KeyValuePair<string, ReferencedNumber>> GetEnumerator()
        {
            foreach (var variable in this.names)
            {
                yield return variable;
            }

            foreach (var name in this.vars)
            {
                yield return name;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
