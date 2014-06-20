using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework.Distributions
{
    public static class Exponential
    {
        // TODO: locate

        //private Random random;

        //private double locate;
        //private double lambda;

        //private double scale;

        //public Exponential(int seed, double locate, double scale)
        //{
        //    this.random = new Random();
        //    this.locate = locate;
        //    this.lambda = 1.0 / scale;
        //}

        //public double GetNext()
        //{
        //    double rand = this.random.NextDouble();
        //    return Math.Log(1 - rand) / (-this.lambda);
        //}

        public static double GetNext(int stream, double locate, double scale)
        {
            double rand = RandomGenerators.GetRandom(stream).NextDouble();
            return -scale * Math.Log(1 - rand);
        }
    }
}
