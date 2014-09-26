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
    public class NamesAndVarsDictionary : IEnumerable<KeyValuePair<string, double>>
    {
        private BiDictionary<string, int> names;
        private Dictionary<string, double> vars;

        public NamesAndVarsDictionary()
        {
            this.names = new BiDictionary<string, int>();
            this.vars = new Dictionary<string, double>();
        }

        public void AddName(string key, int value)
        {
            if (this.names.ContainsByFirst(key) || this.vars.ContainsKey(key))
            {
                throw new ArgumentException("An element with the same key already exists.");
            }

            this.names.Add(key, value);
        }

        public void AddVar(string key, double value)
        {
            if (names.ContainsByFirst(key) || vars.ContainsKey(key))
            {
                throw new ArgumentException("An element with the same key already exists.");
            }

            this.vars.Add(key, value);
        }

        public double GetValue(string key)
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
            if (this.names.ContainsBySecond(value))
            {
                return this.names.GetBySecond(value);
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
            return this.names.ContainsBySecond(value);
        }

        public bool Remove(string key)
        {
            // Full-Or operator
            return this.names.RemoveByFirst(key) | this.vars.Remove(key);
        }

        public IEnumerator<KeyValuePair<string, double>> GetEnumerator()
        {
            foreach (var variable in this.names)
            {
                yield return new KeyValuePair<string, double>(variable.Key, variable.Value);
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
