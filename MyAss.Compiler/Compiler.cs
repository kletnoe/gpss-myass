using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler_v2;
using MyAss.Compiler_v2.CodeGeneration;

namespace MyAss.Compiler
{
    public static class Compiler
    {
        public static IReadOnlyList<string> DefaultRefs
        {
            get
            {
                return new List<string>()
                {
                    "MyAss.Utilities.dll",
                    "MyAss.Framework_v2.dll",
                    "MyAss.Framework_v2.BuiltIn.dll",
                    "MyAss.Framework.Procedures.dll"
                };
            }
        }

        public static IReadOnlyList<string> DefaultNamespaces
        {
            get
            {
                return new List<string>()
                {
                    "MyAss.Framework_v2.BuiltIn.Blocks",
                    "MyAss.Framework_v2.BuiltIn.Commands"
                };
            }
        }

        public static IReadOnlyList<string> DefaultTypes
        {
            get
            {
                return new List<string>()
                {
                    "MyAss.Framework_v2.BuiltIn.SNA.SavevalueSNA",
                    "MyAss.Framework_v2.BuiltIn.SNA.QueueSNA",
                    "MyAss.Framework.Procedures.Distributions"
                };
            }
        }

        public static void Compile(string modelSource)
        {
            Compile(modelSource, new List<string>());
        }

        public static void Compile(string modelSource, List<string> passedRefs)
        {
            Parser_v2 parser = new Parser_v2(new Scanner(new StringCharSource(modelSource)));

            CodeDomGenerationVisitor vis = new CodeDomGenerationVisitor(parser.MetadataRetriever);

            CodeCompileUnit assembly = vis.VisitAll(parser.Model);
        }
    }
}
