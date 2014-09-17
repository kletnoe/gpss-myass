using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("You should pass at least one parameter!");
            }

            string sourceModel = args[0];
            List<string> references = args.Skip(1).ToList();


        }
    }
}
