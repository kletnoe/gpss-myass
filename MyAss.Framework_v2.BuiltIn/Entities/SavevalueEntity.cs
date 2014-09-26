using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework_v2.Entities;

namespace MyAss.Framework_v2.BuiltIn.Entities
{
    public class SavevalueEntity : AbstractEntity
    {
        private Simulation simulation;

        public SavevalueType Value { get; private set; }

        public SavevalueEntity(Simulation simulation, int id)
        {
            this.simulation = simulation;
            this.Id = id;
        }

        public void SetValue(string value)
        {
            this.Value = (SavevalueType)value;
        }

        public string GetValue()
        {
            return this.Value.Value;
        }

        public override void UpdateStats()
        {

        }

        public override string GetStandardReportHeader()
        {
            return String.Format("{0,-24} {1,6} {2,10}",
                    "SAVEVALUE", "RETRY", "VALUE");
        }

        public override string GetStandardReportLine()
        {
            return String.Format("{0,-24} {1,6} {2,10}",
                        this.simulation.NamesDictionary.GetByFirst(this.Id),
                        this.RetryChain.Count,
                        this.Value);
        }

        #region SNA

        public SavevalueType X { get { return this.Value; } }

        #endregion
    }
}
