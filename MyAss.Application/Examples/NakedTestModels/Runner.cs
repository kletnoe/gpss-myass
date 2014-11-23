using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler;

namespace MyAss.Application.Examples.NakedTestModels
{
    public static class Runner
    {
        public static void Run()
        {
            string input = Model_Default.Model;

            AssemblyCompiler compiler = new AssemblyCompiler(input, true);
            compiler.Compile(true);
            compiler.RunAssembly();
        }
    }
}
