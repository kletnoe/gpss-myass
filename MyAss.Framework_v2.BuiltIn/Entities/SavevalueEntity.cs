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
        private SavevalueType value;

        /// <summary>
        /// SNA::X
        /// </summary>
        public SavevalueType Value
        {
            get
            {
                return this.value;
            }
        }

        public SavevalueEntity(Simulation simulation, int id)
        {
            this.simulation = simulation;
            this.Id = id;
        }

        public void SetValue(string value)
        {
            this.value = (SavevalueType)value;
        }

        //public void SubValue(string value)
        //{
        //    this.value = this.value - (SavevalueType)value;
        //}

        //public void AddValue(string value)
        //{
        //    this.value = this.value + (SavevalueType)value;
        //}

        public void SetValueAsDouble(double value)
        {
            this.value = (SavevalueType)value;
        }

        public string GetValue()
        {
            return this.value.Value;
        }

        //public double GetValueAsDouble()
        //{
        //    double number;

        //    if (!Double.TryParse(this.value, out number))
        //    {
        //        number = 0.0;
        //    }

        //    return number;
        //}

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
    }
}
