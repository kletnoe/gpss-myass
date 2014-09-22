using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Framework_v2
{
    public static class TypesDivider<T>
    {
        public static Dictionary<Type, IList<T>> DivideByType(IList<T> inputList)
        {
            Dictionary<Type, IList<T>> resultList = new Dictionary<Type, IList<T>>();

            foreach (var item in inputList)
            {
                Type type = item.GetType();

                if (!resultList.ContainsKey(type))
                {
                    resultList.Add(type, new List<T>());
                }

                resultList[type].Add(item);
            }

            return resultList;
        }
    }
}
