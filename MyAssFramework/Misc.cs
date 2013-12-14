using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework
{
    class Console
    {
        public static void WriteLine(object o, ConsoleColor color)
        {
            System.Console.ForegroundColor = color;
            Console.WriteLine(o);
            System.Console.ResetColor();
        }

        public static void WriteLine(object o)
        {
            System.Console.WriteLine(o);
        }
    }
}
