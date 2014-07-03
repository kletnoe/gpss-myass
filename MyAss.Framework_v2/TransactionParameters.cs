using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace MyAss.Framework_v2
{
    public class TransactionParameters
    {
        private Dictionary<int, double> parameters = new Dictionary<int, double>();

        public double GetParameter(int id)
        {
            double value;

            if (this.parameters.TryGetValue(id, out value))
            {
                return value;
            }
            else
            {
                return Double.NaN;
            }
        }

        public void SetParameter(int id, double value)
        {
            if (this.parameters.ContainsKey(id))
            {
                this.parameters[id] = value;
            }
            else
            {
                this.parameters.Add(id, value);
            }
        }
    }
}
