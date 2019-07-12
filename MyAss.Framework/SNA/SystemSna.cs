using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework.SNA
{
    public static class SystemSna
    {
        public static double MP(Simulation simulation, int parameterId)
        {
            if (!simulation.ActiveTransaction.TransactionParameters.ContainsKey(parameterId))
            {
                throw new ModelingException("Reference to non-existent Parameter: " + simulation.NamesAndVarsDictionary.GetNameByValue(parameterId));
            }

            double result = simulation.Clock - simulation.ActiveTransaction.TransactionParameters[parameterId];
            return result;
        }

        public static double P(Simulation simulation, int parameterId)
        {
            if (!simulation.ActiveTransaction.TransactionParameters.ContainsKey(parameterId))
            {
                throw new ModelingException("Reference to non-existent Parameter: " + simulation.NamesAndVarsDictionary.GetNameByValue(parameterId));
            }

            double result = simulation.ActiveTransaction.TransactionParameters[parameterId];
            return result;
        }
    }
}
