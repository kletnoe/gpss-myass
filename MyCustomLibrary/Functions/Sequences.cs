using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCustomLibrary.Functions
{
    public static class Sequences
    {
        public static int Fibonacci(int number)
        {
            if (number <= 1)
            {
                return number;
            }
            else
            {
                return Fibonacci(number - 2) + Fibonacci(number - 1);
            }
        }
    }
}
