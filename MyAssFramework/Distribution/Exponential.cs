using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.Distributions
{
    public class Exponential : IDistribution
    {
        // TODO: locate

        private Random random;

        private double locate;
        private double lambda; 

        public Exponential(int seed, double locate, double scale)
        {
            this.random = seed == 0 ? new Random() : new Random(seed);
            this.locate = locate;
            this.lambda = 1.0 / scale;
        }

        public double GetNext()
        {
            // return -(1 / lambda) * Math.Log(rand.NextDouble());

            double rand = random.NextDouble();
            return lambda * Math.Pow(Math.E, (lambda) * -rand);
        }
    }
}
