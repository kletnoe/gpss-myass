using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssFramework
{
    public class ModelingException : ApplicationException
    {
        public ModelingException(string message) : base(message){}
    }
}
