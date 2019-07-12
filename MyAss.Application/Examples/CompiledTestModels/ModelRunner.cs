
namespace CompiledTestModels
{
    public static class ModelRunner
    {
        public static void RunMM3()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Restart();

            var sim = new MyAss.Framework.Simulation(new MM3Model_WithTable());

            sw.Stop();
            System.Console.WriteLine("Time elapsed: " + sw.Elapsed);

            MyAss.Utilities.Reports.StandardReport.PrintReport(sim);
            System.Console.WriteLine();
        }
    }
}
