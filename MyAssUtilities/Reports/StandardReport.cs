using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework;
using MyAssFramework.Entities;
using MyAssFramework.Blocks;

namespace MyAssUtilities.Reports
{
    public static class StandardReport
    {
        public static void PrintReport(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(String.Empty.PadRight(80, '='))
                .AppendLine("Start time:\t" + 0)
                .AppendLine("End time:\t" + simulation.Clock)
                .AppendLine("Blocks: \t" + simulation.Blocks.Count)
                .AppendLine("Facilities: \t" + simulation.Entities.Count(x => x.GetType() == typeof(FacilityEntity)))
                .AppendLine("Storages: \t" + simulation.Entities.Count(x => x.GetType() == typeof(StorageEntity)));
                
            sb.AppendLine()
                .AppendLine("// Identificators list should be there")
                .AppendLine();

            // Blocks info
            sb.AppendLine(String.Format("{0,-16}{1,5}{2,16}{3,14}{4,14}{5,14}",
                "Label", "Id", "Block Type", "Entry Count", "Current Count", "Retry"));
            foreach (IBlock block in simulation.Blocks)
            {
                sb.AppendLine(String.Format("{0,-16}{1,5}{2,16}{3,14}{4,14}{5,14}",
                    "", 
                    block.Id, 
                    block.GetType().Name, 
                    block.EntryCount, 
                    "", 
                    block.RetryChain.Count));
            }

            Console.WriteLine(sb);
        }
    }
}
