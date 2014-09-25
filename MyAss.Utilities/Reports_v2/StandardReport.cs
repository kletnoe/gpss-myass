using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework_v2;
using MyAss.Framework_v2.Blocks;
//using MyAss.Framework_v2.BuiltIn.Entities;
using MyAss.Framework_v2.Entities;

namespace MyAss.Utilities.Reports_v2
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
            sb.AppendLine(EntitiesInfo(simulation));
            ///sb.Append(FacilitiesInfo(simulation));
            //sb.Append(QueuesInfo(simulation));
            //sb.Append(StoragesInfo(simulation));
            //sb.Append(TablesInfo(simulation));
            //sb.Append(UserChainsInfo(simulation));
            //sb.Append(TransactionGroupsInfo(simulation));
            //sb.Append(NumericGroupsInfo(simulation));
            //sb.Append(LogicswitchesInfo(simulation));
            //sb.Append(SavevaluesInfo(simulation));
            //sb.Append(MatrixEntitiesInfo(simulation));

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
                ///.AppendLine("Facilities: \t" + simulation.Entities.Count(x => x.GetType() == typeof(FacilityEntity)))
                ///.AppendLine("Storages: \t" + simulation.Entities.Count(x => x.GetType() == typeof(StorageEntity)))
                ;

            return sb.ToString();
        }

        private static String NamesInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine().AppendLine();
            sb.AppendLine(String.Format("{0, 10} {1,-24} {2,6}",
                    String.Empty, "NAME", "VALUE"));
            foreach (var name in simulation.NamesDictionary.SecondToFirst)
            {
                sb.AppendLine(String.Format("{0, 10} {1,-24} {2,6}",
                    String.Empty,
                    name.Key,
                    name.Value));
            }
            return sb.ToString();
        }

        private static String BlocksInfo(Simulation simulation)
        {
            // TODO: Refactor this:
            List<Transaction> allTransactions = new List<Transaction>();
            allTransactions.AddRange(simulation.CurrentEventChain);
            allTransactions.AddRange(simulation.FutureEventChain);
            allTransactions.AddRange(simulation.Entities.Values.OfType<IDelayableEntity>().SelectMany(x => x.DelayChain));
            allTransactions.AddRange(simulation.Entities.Values.OfType<IPendableEntity>().SelectMany(x => x.PendingChain));
            //

            StringBuilder sb = new StringBuilder();

            sb.AppendLine().AppendLine();
            sb.AppendLine(String.Format("{0,-15} {1,5} {2,15} {3,13} {4,13} {5,13}",
                "Label", "Id", "Block Type", "Entry Count", "Current Count", "Retry"));
            foreach (var block in simulation.Blocks)
            {
                sb.AppendLine(String.Format("{0,-15} {1,5} {2,15} {3,13} {4,13} {5,13}",
                    simulation.NamesDictionary.ContainsByFirst(block.Value.Id) ? simulation.NamesDictionary.GetByFirst(block.Value.Id) : String.Empty,
                    block.Value.Id,
                    block.Value.GetType().Name,
                    block.Value.EntryCount,
                    allTransactions.Count(x => x.Owner == block.Value.Id),
                    block.Value.RetryChain.Count));
            }

            return sb.ToString();
        }

        private static String EntitiesInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();

            IList<IEntity> entities = simulation.Entities.Values.ToList();
            Dictionary<Type, IList<IEntity>> entityTypeToEntitiesList = TypesDivider<IEntity>.DivideByType(entities);

            foreach (var singleTypeEntities in entityTypeToEntitiesList)
            {
                if (singleTypeEntities.Value.Any())
                {
                    sb.AppendLine().AppendLine();
                    sb.AppendLine(singleTypeEntities.Value.First().GetStandardReportHeader());

                    foreach (var entity in singleTypeEntities.Value)
                    {
                        sb.AppendLine(entity.GetStandardReportLine());
                    }
                }
            }

            return sb.ToString();
        }

        private static String CECInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine().AppendLine();
            sb.AppendLine(String.Format("{0,-6} {1,4} {2,10} {3,8} {4,8} {5,6} {6, 15} {7, 10}",
                "CEC XN", "PRI", "M1", "ASSEM", "CURRENT", "NEXT", "PARAMETER", "VALUE"));

            foreach (Transaction tr in simulation.CurrentEventChain)
            {
                sb.AppendLine(String.Format("{0,6} {1,4} {2,10} {3,8} {4,8} {5,6}",
                    tr.Number,
                    tr.Priority,
                    tr.MarkTime.ToString("F3"),
                    tr.AssemblySet,
                    tr.Owner,
                    tr.NextOwner));

                foreach (var parameter in tr.TransactionParameters)
                {
                    sb.AppendLine(String.Format("{0,47} {1,15} {2,10}",
                        String.Empty,
                        simulation.NamesDictionary.GetByFirst(parameter.Key),
                        parameter.Value.ToString("F3")));
                }
            }

            return sb.ToString();
        }

        private static String FECInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine().AppendLine();
            sb.AppendLine(String.Format("{0,-6} {1,4} {2,10} {3,8} {4,8} {5,6} {6, 15} {7, 10}",
                "FEC XN", "PRI", "BDT", "ASSEM", "CURRENT", "NEXT", "PARAMETER", "VALUE"));

            foreach (Transaction tr in simulation.FutureEventChain)
            {
                sb.AppendLine(String.Format("{0,6} {1,4} {2,10} {3,8} {4,8} {5,6}",
                    tr.Number,
                    tr.Priority,
                    tr.NextEventTime.ToString("F3"),
                    tr.AssemblySet,
                    tr.Owner,
                    tr.NextOwner));

                foreach (var parameter in tr.TransactionParameters)
                {
                    sb.AppendLine(String.Format("{0,47} {1,15} {2,10}",
                        String.Empty,
                        simulation.NamesDictionary.GetByFirst(parameter.Key),
                        parameter.Value.ToString("F3")));
                }
            }

            return sb.ToString();
        }
    }
}
