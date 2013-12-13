using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.Distributions
{
    public class Uniform : IDistribution
    {
        private Random random;

        private double minValue;
        private double maxValue;

        public Uniform(int seed, double minValue, double maxValue)
        {
            this.random = seed == 0 ? new Random() : new Random(seed);
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public double GetNext()
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }
    }
}
