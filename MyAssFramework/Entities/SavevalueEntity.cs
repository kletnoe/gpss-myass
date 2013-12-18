using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework.Entities
{
    public class SavevalueEntity : AbstractEntity
    {
        private SavevalueType value;

        public SavevalueType Value
        {
            get
            {
                return this.value;
            }
        }

        public SavevalueEntity(int id, string value)
        {
            this.Id = id;
            this.value = (SavevalueType)value;
        }

        public void SetValue(string value)
        {
            this.value = (SavevalueType)value;
        }

        public void SubValue(string value)
        {
            this.value = this.value - (SavevalueType)value;
        }

        public void AddValue(string value)
        {
            this.value = this.value + (SavevalueType)value;
        }

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
    }
}
