using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.Distributions
{
    public class Normal
    {
        private Random random;

        private double mu;
        private double sigma;

        public Normal(int seed, double mu, double sigma)
        {
            this.random = seed == 0 ? new Random() : new Random(seed);
            this.mu = mu;
            this.sigma = sigma;
        }

        public double GetNext()
        {
            const int n = 1000;
            double result = 0;
            for (int i = 0; i < n; i++)
            {
                result += random.NextDouble();
            }
            result = (result - (n / 2)) / Math.Sqrt(n / 12d);
            result = result * sigma + mu;

            return result;
        }
    }
}
