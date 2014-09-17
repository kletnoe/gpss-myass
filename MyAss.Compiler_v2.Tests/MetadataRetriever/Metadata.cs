using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler.Metadata;
using NUnit.Framework;

namespace MyAss.Compiler_v2.Tests.MetadataRetriever
{
    [TestFixture]
    [Category("MetadataTests_v2")]
    public class Metadata
    {
        [Test]
        public void Test_Metadata()
        {
            List<string> assemblies = new List<string>()
            {
                "MyAss.Framework_v2.BuiltIn.dll",
                "MyAss.Framework.Procedures.dll"
            };

            List<string> namespaces = new List<string>()
            {
                "MyAss.Framework_v2.BuiltIn.Blocks",
                "MyAss.Framework_v2.BuiltIn.Commands"
            };

            List<string> types = new List<string>()
            {
                "MyAss.Framework_v2.BuiltIn.SNA.SavevalueSNA",
                "MyAss.Framework_v2.BuiltIn.SNA.QueueSNA",
                "MyAss.Framework.Procedures.Distributions"
            };

            MetadataRetriever_v2 mdr = new MetadataRetriever_v2(assemblies, namespaces, types);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Verbs:");

            foreach (var verb in mdr.Verbs)
            {
                sb.AppendLine(verb.Name);
            }

            sb.AppendLine();
            sb.AppendLine("Procedures:");

            foreach (var procedure in mdr.Procedures)
            {
                sb.AppendLine(procedure.Name);
            }

            Assert.Pass(sb.ToString());
        }
    }
}
