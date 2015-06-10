using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework_v2
{
    public class ReferencedNumber
    {
        public bool IsInt { get; private set; }
        public int IntValue { get; private set; }
        public double DoubleValue { get; private set; }

        public static implicit operator ReferencedNumber(int val)
        {
            return new ReferencedNumber(val);
        }

        public static implicit operator double (ReferencedNumber d)
        {
            if (d.IsInt)
            {
                return d.IntValue;
            }
            else
            {
                return d.DoubleValue;
            }
        }

        public static implicit operator int (ReferencedNumber d)
        {
            if (d.IsInt)
            {
                return d.IntValue;
            }
            else
            {
                return (int)d.DoubleValue;
            }
        }

        public ReferencedNumber(int value)
        {
            this.IsInt = true;
            this.IntValue = value;

            this.DoubleValue = Double.NaN;
        }

        public ReferencedNumber(double value)
        {
            this.IsInt = false;
            this.DoubleValue = value;

            this.IntValue = 0;
        }

        public void SetValueInt(int value)
        {
            this.IsInt = false;
            this.DoubleValue = value;

            this.DoubleValue = Double.NaN;
        }

        public void SetValueDouble(double value)
        {
            this.IsInt = false;
            this.DoubleValue = value;

            this.IntValue = 0;
        }

        #region overrides

        public override int GetHashCode()
        {
            if (IsInt)
            {
                return this.IntValue;
            }
            else
            {
                throw new Exception("HashCode plug!");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            ReferencedNumber other = obj as ReferencedNumber;
            if (other == null)
            {
                return false;
            }

            if (this.IsInt == other.IsInt)
            {
                if (this.IsInt)
                {
                    return this.IntValue == other.IntValue;
                }
                else
                {
                    return this.DoubleValue == other.DoubleValue;
                }
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            if (this.IsInt)
            {
                return this.IntValue.ToString();
            }
            else
            {
                return this.DoubleValue.ToString();
            }
        }

        #endregion
    }
}
