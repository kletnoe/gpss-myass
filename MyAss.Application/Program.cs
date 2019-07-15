using System;
using MyAss.Application.Examples.CompiledTestModels;
using MyAss.Application.Examples.NakedTestModels;

namespace MyAss.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            //CompiledModelRunner.Run(new Examples.CompiledTestModels.MM3Model());
            //CompiledModelRunner.Run(new Examples.CompiledTestModels.MM3Model_Dynamic());
            //CompiledModelRunner.Run(new Examples.CompiledTestModels.MM3Model_WithTable());
            //CompiledModelRunner.Run(new Examples.CompiledTestModels.Default.TheModel());
            //CompiledModelRunner.Run(new Examples.CompiledTestModels.Turnstale.TheModel());


            //Runner.Run(Examples.NakedTestModels.Model_Default.Model);
            //Runner.Run(Examples.NakedTestModels.Model_DefaultNoQueue.Model);
            Runner.Run(Examples.NakedTestModels.Model_DefaultWithTable.Model);
            //Runner.Run(Examples.NakedTestModels.Model_DefaultWithTimeout.Model);
            //Runner.Run(Examples.NakedTestModels.Model_Lusin.Model);
            //Runner.Run(Examples.NakedTestModels.Model_MM5_Compare.Model);
            //Runner.Run(Examples.NakedTestModels.Model_TurnstaleDemo.Model);
            //Runner.Run(Examples.NakedTestModels.Model_WithEQU.Model);

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}

