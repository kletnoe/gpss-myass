using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework.Procedures
{
    public static class Math
    {
        // Absolute value
        public static double ABS(double expression)
        {
            return System.Math.Abs(expression);
        }

        // Arctangent in radians
        public static double ATN(double expression)
        {
            return System.Math.Atan(expression);
        }

        // Cosine. Expression must be in radians
        public static double COS(double expression)
        {
            return System.Math.Cos(expression);
        }

        // e raised to the power given by Expression
        public static double EXP(double expression)
        {
            return System.Math.Exp(expression);
        }

        // Truncation toward zero.
        public static double INT(double expression)
        {
            return System.Math.Floor(expression);
        }

        // Natural logarithm.
        public static double LOG(double expression)
        {
            return System.Math.Log(expression);
        }

        // Sine. Expression must be in radians.
        public static double SIN(double expression)
        {
            return System.Math.Sin(expression);
        }

        // Square Root.
        public static double SQR(double expression)
        {
            return System.Math.Sqrt(expression);
        }

        // Tangent. Expression must be in radians.
        public static double TAN(double expression)
        {
            return System.Math.Tan(expression);
        }
    }
}
