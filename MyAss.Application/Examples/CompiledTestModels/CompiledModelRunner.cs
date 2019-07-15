using MyAss.Framework;

namespace MyAss.Application.Examples.CompiledTestModels
{
    public static class CompiledModelRunner
    {
        public static void Run(AbstractModel compileModel)
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Restart();

            var sim = new MyAss.Framework.Simulation(compileModel);

            sw.Stop();
            System.Console.WriteLine("Time elapsed: " + sw.Elapsed);

            MyAss.Utilities.Reports.StandardReport.PrintReport(sim);
            System.Console.WriteLine();
        }
    }
}
