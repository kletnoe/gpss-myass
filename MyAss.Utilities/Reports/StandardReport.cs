using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework;
using MyAss.Framework.Blocks;
//using MyAss.Framework.BuiltIn.Entities;
using MyAss.Framework.Entities;

namespace MyAss.Utilities.Reports
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
            sb.AppendLine(String.Format("{0, 10} {1,-24} {2,10}",
                    String.Empty, "NAME", "VALUE"));
            foreach (var name in simulation.NamesAndVarsDictionary)
            {
                sb.AppendLine(String.Format("{0, 10} {1,-24} {2,10}",
                    String.Empty,
                    name.Key,
                    name.Value.ToString()));
            }
            return sb.ToString();
        }

        private static String BlocksInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine().AppendLine();
            sb.AppendLine(String.Format("{0,-15} {1,5} {2,15} {3,13} {4,13} {5,13}",
                "Label", "Id", "Block Type", "Entry Count", "Current Count", "Retry"));
            foreach (var block in simulation.Blocks)
            {
                sb.AppendLine(String.Format("{0,-15} {1,5} {2,15} {3,13} {4,13} {5,13}",
                    simulation.NamesAndVarsDictionary.ContainsNameValue(block.Value.Id) ? simulation.NamesAndVarsDictionary.GetNameByValue(block.Value.Id) : String.Empty,
                    block.Value.Id,
                    block.Value.GetType().Name,
                    block.Value.EntryCount,
                    block.Value.CurrentCount,
                    block.Value.RetryChain.Count));
            }

            return sb.ToString();
        }

        private static String EntitiesInfo(Simulation simulation)
        {
            StringBuilder sb = new StringBuilder();

            IList<AnyEntity> entities = simulation.Entities.Values.ToList();
            Dictionary<Type, IList<AnyEntity>> entityTypeToEntitiesList = TypesDivider<AnyEntity>.DivideByType(entities);

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
                    tr.Owner != null ? tr.Owner.Id : 0,
                    tr.NextOwner.Id));

                foreach (var parameter in tr.TransactionParameters)
                {
                    sb.AppendLine(String.Format("{0,47} {1,15} {2,10}",
                        String.Empty,
                        simulation.NamesAndVarsDictionary.GetNameByValue(parameter.Key),
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
                    tr.Owner != null ? tr.Owner.Id : 0,
                    tr.NextOwner.Id));

                foreach (var parameter in tr.TransactionParameters)
                {
                    sb.AppendLine(String.Format("{0,47} {1,15} {2,10}",
                        String.Empty,
                        simulation.NamesAndVarsDictionary.GetNameByValue(parameter.Key),
                        parameter.Value.ToString("F3")));
                }
            }

            return sb.ToString();
        }
    }
}
