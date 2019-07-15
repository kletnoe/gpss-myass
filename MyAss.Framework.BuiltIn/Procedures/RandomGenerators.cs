using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace MyAss.Framework.BuiltIn.Procedures
{
    public static class RandomGenerators
    {
        private static Dictionary<int, Random> generators = new Dictionary<int, Random>();

        public static Random GetRandom(int stream)
        {
            Random rand;

            if (RandomGenerators.generators.TryGetValue(stream, out rand))
            {
                return rand;
            }
            else
            {
                rand = stream == 0 ? new Random() : new Random(stream);
                RandomGenerators.generators.Add(stream, rand);
                return rand;
            }
        }
    }
}
