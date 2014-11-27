using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MyAss.Compiler_v2.AST
{
    [DataContract]
    public abstract class ASTAnyNode : IASTVisitableNode
    {
        public abstract T Accept<T>(IASTVisitor<T> visitor);

        public string Serialize()
        {
            String result;

            var serializer = new DataContractSerializer(this.GetType());
            var settings = new XmlWriterSettings
            {
                Indent = true,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
                Encoding = Encoding.UTF8,

            };
            using (var sw = new StringWriter())
            {
                using (var xw = XmlWriter.Create(sw, settings))
                {
                    serializer.WriteStartObject(xw, this);
                    xw.WriteAttributeString("xmlns", "xs", null, "http://www.w3.org/2001/XMLSchema");
                    serializer.WriteObjectContent(xw, this);
                    serializer.WriteEndObject(xw);
                    //serializer.WriteObject(xw, node);
                }
                result = sw.ToString();
            }

            return result;
        }
    }
}
