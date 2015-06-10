using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler.Metadata
{
    public class ClassMeta
    {
        public string Assembly { get; private set; }
        public string Namespace { get; private set; }
        public string Class { get; private set; }

        public ClassMeta(string theAssembly, string theNamespace, string theClass)
        {
            this.Assembly = theAssembly;
            this.Namespace = theNamespace;
            this.Class = theClass;
        }
    }
}
