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

            sb.AppendLine(String.Empty.PadRight(75, '='));
            sb.AppendLine(String.Empty.PadRight(75, '='));

            // General
            sb.Append(GeneralInfo(simulation));
            sb.Append(NamesInfo(simulation));
            sb.Append(BlocksInfo(simulation));

            // Entities
            sb.Append(FacilitiesInfo(simulation));
            sb.Append(QueuesInfo(simulation));
            sb.Append(StoragesInfo(simulation));
            sb.Append(TablesInfo(simulation));
            sb.Append(UserChainsInfo(simulation));
            sb.Append(TransactionGroupsInfo(simulation));
            sb.Append(NumericGroupsInfo(simulation));
            sb.Append(LogicswitchesInfo(simulation));
            sb.Append(SavevaluesInfo(simulation));
            sb.Append(MatrixEntitiesInfo(simulation));

            // Transaction Chains
            sb.Append(CECInfo(simulation));
            sb.Append(FECInfo(simulation));

            Console.WriteLine(sb);
        }

        private static String GeneralInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Start time:\t" + 0)
                .AppendLine("End time:\t" + simulation.Clock)
                .AppendLine("Blocks: \t" + simulation.Blocks.Count)
                .AppendLine("Facilities: \t" + simulation.Entities.Count(x => x.GetType() == typeof(FacilityEntity)))
                .AppendLine("Storages: \t" + simulation.Entities.Count(x => x.GetType() == typeof(StorageEntity)));

            return sb.ToString();
        }

        private static String NamesInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// Identificators list should be there");
            return sb.ToString();
        }

        private static String BlocksInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine().AppendLine();
            sb.AppendLine(String.Format("{0,-15} {1,5} {2,15} {3,13} {4,13} {5,13}",
                "Label", "Id", "Block Type", "Entry Count", "Current Count", "Retry"));
            foreach (IBlock block in simulation.Blocks)
            {
                sb.AppendLine(String.Format("{0,-15} {1,5} {2,15} {3,13} {4,13} {5,13}",
                    "",
                    block.Id,
                    block.GetType().Name,
                    block.EntryCount,
                    simulation.CurrentEventChain.Count(x => x.Owner == block)
                        + simulation.FutureEventChain.Count(x => x.Owner == block),
                    block.RetryChain.Count));
            }

            return sb.ToString();
        }

        private static String FacilitiesInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }

        private static String QueuesInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();
            IList<QueueEntity> items = simulation.Entities.OfType<QueueEntity>().ToList();

            if (items.Count > 0)
            {
                sb.AppendLine().AppendLine();
                sb.AppendLine(String.Format("{0,-14} {1,6} {2,6} {3,6} {4,6} {5,9} {6,9} {7,9} {8,6}",
                    "QUEUE", "MAX", "CONT.", "ENTRY", "ENTRY0", "AVE.CONT", "AVE.TIME", "AVE.-0", "RETRY"));

                foreach (var item in items)
                {
                    sb.AppendLine(String.Format("{0,-14} {1,6} {2,6} {3,6} {4,6} {5,9} {6,9} {7,9} {8,6}",
                        item.Id,
                        item.MaxContent,
                        item.CurrentContent,
                        item.EntriesCount,
                        "ToDo",
                        "ToDo",
                        "ToDo",
                        "ToDo",
                        "ToDo"));
                }
            }
            return sb.ToString();
        }

        private static String StoragesInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();
            IList<StorageEntity> items = simulation.Entities.OfType<StorageEntity>().ToList();

            if (items.Count > 0)
            {
                sb.AppendLine().AppendLine();
                sb.AppendLine(String.Format("{0,-14} {1,4} {2,4} {3,4} {4,4} {5,6} {6,4} {7,9} {8,6} {9,4} {10,4}",
                    "STOTAGE", "CAP.", "REM.", "MIN.", "MAX.", "ENTRY", "AVL.", "AVE.C", "UTIL", "RETRY", "DELAY"));

                foreach (var item in items)
                {
                    sb.AppendLine(String.Format("{0,-14} {1,4} {2,4} {3,4} {4,4} {5,6} {6,4} {7,9} {8,6} {9,4} {10,4}",
                        item.Id,
                        item.Capacity,
                        item.RemainingCapacity,
                        item.MinContent,
                        item.MaxContent,
                        item.EntriesCount,
                        "ToDo",
                        "ToDo",
                        "ToDo",
                        "ToDo",
                        item.DelayChain.Count));
                }
            }

            return sb.ToString();
        }

        private static String TablesInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }

        private static String UserChainsInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }

        private static String TransactionGroupsInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }

        private static String NumericGroupsInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }

        private static String LogicswitchesInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }

        private static String SavevaluesInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();
            IList<SavevalueEntity> items = simulation.Entities.OfType<SavevalueEntity>().ToList();

            if (items.Count > 0)
            {
                sb.AppendLine().AppendLine();
                sb.AppendLine(String.Format("{0,-24} {1,6} {2,10}",
                    "SAVEVALUE", "RETRY", "VALUE"));

                foreach (var item in items)
                {
                    sb.AppendLine(String.Format("{0,-24} {1,6} {2,10}",
                        item.Id,
                        "ToDo",
                        item.Value));
                }
            }

            return sb.ToString();
        }

        private static String MatrixEntitiesInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }

        private static String CECInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine().AppendLine();
            sb.AppendLine(String.Format("{0,-6} {1,6} {2,10} {3,10} {4,10} {5,10}",
                "CEC XN", "PRI", "M1", "ASSEM", "CURRENT", "NEXT"));

            foreach (Transaction tr in simulation.CurrentEventChain)
            {
                sb.AppendLine(String.Format("{0,6} {1,6} {2,10} {3,10} {4,10} {5,10}",
                    tr.AssemblySet,
                    tr.Priority,
                    tr.MarkTime,
                    tr.AssemblySet,
                    tr.Owner.Id,
                    "ToDo"));
            }

            return sb.ToString();
        }

        private static String FECInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine().AppendLine();
            sb.AppendLine(String.Format("{0,-6} {1,6} {2,10} {3,10} {4,10} {5,10}",
                "FEC XN", "PRI", "BDT", "ASSEM", "CURRENT", "NEXT"));

            foreach (Transaction tr in simulation.CurrentEventChain)
            {
                sb.AppendLine(String.Format("{0,6} {1,6} {2,10} {3,10} {4,10} {5,10}",
                    tr.AssemblySet,
                    tr.Priority,
                    tr.NextEventTime,
                    tr.AssemblySet,
                    tr.Owner.Id,
                    "ToDo"));
            }

            return sb.ToString();
        }
    }
}
