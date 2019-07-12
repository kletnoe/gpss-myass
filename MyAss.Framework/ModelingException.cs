using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Framework
{
    public class ModelingException : ApplicationException
    {
        public ModelingException(string message) : base(message){}
    }
}
