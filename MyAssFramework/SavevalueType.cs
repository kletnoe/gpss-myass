using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework
{
    public class SavevalueType
    {
        private string value;

        public SavevalueType()
        {

        }

        private SavevalueType(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get
            {
                return value;
            }
        }

        #region ConversionOperators

        public static explicit operator SavevalueType(string value)
        {
            return new SavevalueType(value);
        }

        public static explicit operator SavevalueType(double value)
        {
            return new SavevalueType(value.ToString());
        }

        public static explicit operator String(SavevalueType value)
        {
            return value.Value;
        }

        public static explicit operator Double(SavevalueType value)
        {
            double result;
            Double.TryParse(value.Value, out result);
            return result;
        }

        #endregion

        #region OperatorOverloads

        public static SavevalueType operator +(SavevalueType lValue, SavevalueType rValue)
        {
            string result = String.Empty;

            double lDouble;
            double rDouble;
            if (Double.TryParse(lValue.Value, out lDouble) && Double.TryParse(rValue.Value, out rDouble))
            {
                result = (lDouble + rDouble).ToString();
            }
            else
            {
                result = lValue.Value + rValue.Value;
            }

            return new SavevalueType(result);
        }

        public static SavevalueType operator -(SavevalueType lValue, SavevalueType rValue)
        {
            string result = String.Empty;

            double lDouble;
            double rDouble;
            if (Double.TryParse(lValue.Value, out lDouble) && Double.TryParse(rValue.Value, out rDouble))
            {
                result = (lDouble - rDouble).ToString();
            }

            return new SavevalueType(result);
        }

        public static SavevalueType operator *(SavevalueType lValue, SavevalueType rValue)
        {
            string result = String.Empty;

            double lDouble;
            double rDouble;
            if (Double.TryParse(lValue.Value, out lDouble) && Double.TryParse(rValue.Value, out rDouble))
            {
                result = (lDouble * rDouble).ToString();
            }

            return new SavevalueType(result);
        }

        public static SavevalueType operator /(SavevalueType lValue, SavevalueType rValue)
        {
            string result = String.Empty;

            double lDouble;
            double rDouble;
            if (Double.TryParse(lValue.Value, out lDouble) && Double.TryParse(rValue.Value, out rDouble))
            {
                result = (lDouble / rDouble).ToString();
            }

            return new SavevalueType(result);
        }

        #endregion
    }
}
