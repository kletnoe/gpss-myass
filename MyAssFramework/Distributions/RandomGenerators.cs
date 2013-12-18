using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace MyAssFramework.Distributions
{
    public static class RandomGenerators
    {
        private static Dictionary<int, Random> generators = new Dictionary<int, Random>();

        public static Random GetRandom(int stream)
        {
            Random rand;

            if (generators.TryGetValue(stream, out rand))
            {
                return rand;
            }
            else
            {
                rand = stream == 0 ? new Random() : new Random(stream);
                generators.Add(stream, rand);
                return rand;
            }
        }
    }
}
