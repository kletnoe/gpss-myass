using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework.v2_Types
{
    public struct InnerDynamicType
    {
        public enum ValueType
        {
            String,
            Int32,
            Double
        }

        private int intValue;
        private double doubleValue;
        private string stringValue;
        private ValueType currentType;

        public ValueType Type { get { return this.currentType; } }
        public int IntValue { get { return this.intValue; } }
        public double DoubleValue { get { return this.doubleValue; } }
        public string StringValue { get { return this.stringValue; } }

        public InnerDynamicType(int value)
        {
            this.currentType = ValueType.Int32;
            this.intValue = value;

            this.doubleValue = 0;
            this.stringValue = string.Empty;
        }

        public InnerDynamicType(double value)
        {
            this.currentType = ValueType.Double;
            this.doubleValue = value;

            this.intValue = 0;
            this.stringValue = string.Empty;
        }

        public InnerDynamicType(string value)
        {
            this.currentType = ValueType.String;
            this.stringValue = value;

            this.intValue = 0;
            this.doubleValue = 0;
        }

        #region OperatorOverloads

        public static InnerDynamicType operator +(InnerDynamicType lValue, InnerDynamicType rValue)
        {
            switch (lValue.currentType)
            {
                case ValueType.String:
                    switch (rValue.currentType)
                    {
                        case ValueType.String:
                            {
                                double lDouble;
                                Double.TryParse(lValue.stringValue, out lDouble);
                                double rDouble;
                                Double.TryParse(rValue.stringValue, out rDouble);
                                return new InnerDynamicType(lDouble + rDouble);
                            }
                        case ValueType.Int32:
                            {
                                double lDouble;
                                Double.TryParse(lValue.stringValue, out lDouble);
                                return new InnerDynamicType(lDouble + rValue.intValue);
                            }
                        case ValueType.Double:
                            {
                                double lDouble;
                                Double.TryParse(lValue.stringValue, out lDouble);
                                return new InnerDynamicType(lDouble + rValue.doubleValue);
                            }

                        default: throw new Exception();
                    }

                case ValueType.Int32:
                    switch (rValue.currentType)
                    {
                        case ValueType.String:
                            {
                                double rDouble;
                                Double.TryParse(rValue.stringValue, out rDouble);
                                return new InnerDynamicType(rValue.intValue + rDouble);
                            }
                        case ValueType.Int32:
                            return new InnerDynamicType(lValue.intValue + rValue.intValue);
                        case ValueType.Double:
                            return new InnerDynamicType(lValue.intValue + rValue.doubleValue);

                        default: throw new Exception();
                    }

                case ValueType.Double:
                    switch (rValue.currentType)
                    {
                        case ValueType.String:
                            {
                                double rDouble;
                                Double.TryParse(rValue.stringValue, out rDouble);
                                return new InnerDynamicType(rValue.doubleValue + rDouble);
                            }
                        case ValueType.Int32:
                            return new InnerDynamicType(lValue.doubleValue + rValue.intValue);
                        case ValueType.Double:
                            return new InnerDynamicType(lValue.doubleValue + rValue.doubleValue);

                        default: throw new Exception();
                    }

                default: throw new Exception();
            }
        }

        #endregion
    }
}
