using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Compiler.AST;
using MyAss.Compiler;
using MyAss.Compiler.CodeGeneration;
using System.CodeDom;
using CompiledTestModels;
using MyAss.Application.Examples.NakedTestModels;

namespace MyAss.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            Runner.Run();
            Console.WriteLine("Done");
        }
    }
}

