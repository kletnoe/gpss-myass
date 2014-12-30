using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler.Metadata
{
    public class MethodMeta
    {
        public string Assembly { get; private set; }
        public string Namespace { get; private set; }
        public string Class { get; private set; }
        public string Name { get; private set; }

        public MethodMeta(string theAssembly, string theNamespace, string theClass, string theName)
        {
            this.Assembly = theAssembly;
            this.Namespace = theNamespace;
            this.Class = theClass;
            this.Name = theName;
        }
    }
}
