using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2;
using MyAss.Utilities.Reports_v2;

namespace MyAss.Application.Examples.TestModels
{
    public static class ModelRunner
    {
        public static void RunMM3()
        {
            Simulation sim = new Simulation(new MM3Model().Construct());
            StandardReport.PrintReport(sim);
        }
    }
}
