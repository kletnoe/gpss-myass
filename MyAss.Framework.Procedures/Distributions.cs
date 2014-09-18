using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework.Procedures
{
    public static class Distributions
    {
        // 1. Beta
        public static double Beta(double stream, double min, double max, double shape1, double shape2)
        {
            throw new NotImplementedException();
        }

        // 2. Binomial
        public static double Binomial(double stream, double trialCount, double probability)
        {
            throw new NotImplementedException();
        }

        // 3. Discrete Uniform
        public static double DUniform(double stream, double min, double max)
        {
            throw new NotImplementedException();
        }

        // 4. Exponential
        public static double Exponential(double stream, double locate, double scale)
        {
            int istream = (int)stream;

            // TODO: Locate is not supported

            double randValue = RandomGenerators.GetRandom(istream).NextDouble();
            return -scale * System.Math.Log(1 - randValue);
        }

        // 5. Extreme Value A
        public static double ExtValA(double stream, double locate, double scale)
        {
            throw new NotImplementedException();
        }

        // 6. Extreme Value B
        public static double ExtValB(double stream, double locate, double scale)
        {
            throw new NotImplementedException();
        }

        // 7. Gamma
        public static double Gamma(double stream, double locate, double scale, double shape)
        {
            throw new NotImplementedException();
        }

        // 8. Geometric
        public static double Geometric(double stream, double probability)
        {
            throw new NotImplementedException();
        }

        // 9. Inverse Gaussian
        public static double InvGauss(double stream, double locate, double scale, double shape)
        {
            throw new NotImplementedException();
        }

        // 10. Inverse Weibull
        public static double InvWeibull(double stream, double locate, double scale, double shape)
        {
            throw new NotImplementedException();
        }

        // 11. Laplace
        public static double Laplace(double stream, double locate, double scale)
        {
            throw new NotImplementedException();
        }

        // 12. Logistic
        public static double Logistic(double stream, double locate, double scale)
        {
            throw new NotImplementedException();
        }

        // 13. LogLaplace
        public static double LogLaplace(double stream, double locate, double scale, double shape)
        {
            throw new NotImplementedException();
        }

        // 14. LogLogistic
        public static double LogLogis(double stream, double locate, double scale, double shape)
        {
            throw new NotImplementedException();
        }

        // 15. LogNormal
        public static double LogNormal(double stream, double locate, double scale, double shape)
        {
            throw new NotImplementedException();
        }

        // 16. Negative Binomial
        public static double NegBinom(int Sstream, double successCount, double probability)
        {
            throw new NotImplementedException();
        }

        // 17. Normal
        public static double Normal(double stream, double mean, double stdDev)
        {
            int istream = (int)stream;
            Random rand = RandomGenerators.GetRandom(istream);

            const int n = 1000;
            double result = 0;
            for (int i = 0; i < n; i++)
            {
                result += rand.NextDouble();
            }
            result = (result - (n / 2)) / System.Math.Sqrt(n / 12d);
            result = result * stdDev + mean;

            return result;
        }

        // 18. Pareto
        public static double Pareto(double stream, double locate, double scale)
        {
            throw new NotImplementedException();
        }

        // 19. Pearson Type V
        public static double Pearson5(double stream, double locate, double scale, double shape)
        {
            throw new NotImplementedException();
        }

        // 20. Pearson Type VI
        public static double Pearson6(double stream, double locate, double scale, double shape1, double shape2)
        {
            throw new NotImplementedException();
        }

        // 21. Poisson
        public static double Poisson(double stream, double mean)
        {
            throw new NotImplementedException();
        }

        // 22. Triangular
        public static double Triangular(double stream, double min, double max, double mode)
        {
            throw new NotImplementedException();
        }

        // 23. Uniform
        public static double Uniform(double stream, double min, double max)
        {
            int istream = (int)stream;

            double randValue = RandomGenerators.GetRandom(istream).NextDouble();
            return randValue * (max - min) + min;
        }

        // 24. Weibull
        public static double Weibull(double stream, double locate, double scale, double shape)
        {
            throw new NotImplementedException();
        }
    }
}
