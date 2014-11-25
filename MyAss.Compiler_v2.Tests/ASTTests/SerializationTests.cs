using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using MyAss.Compiler;
using MyAss.Compiler_v2.AST;
using NUnit.Framework;

namespace MyAss.Compiler_v2.Tests.ASTTests
{
    [TestFixture]
    [Category("AST_Setialization")]
    public class SerializationTests
    {
        [Test]
        public void Model_Standart()
        {
            string input = TestModels.MM3Model_Simple;
            ASTAnyNode node = this.RunModel(input);
            string result = this.Serialize(node, node.GetType());
            Console.WriteLine(result);
        }

        [Test]
        public void Model()
        {
            ASTModel node = new ASTModel();
            node.Verbs.Add(new ASTVerb());
            node.Verbs.Add(new ASTVerb());

            string result = this.Serialize(node, node.GetType());
            Console.WriteLine(result);
        }

        [Test]
        public void Verb()
        {
            ASTVerb node = new ASTVerb();
            node.LabelId = "SomeLabel";
            node.VerbId = "START";
            node.Operands.Add(new ASTLiteral()
            {
                LiteralType = LiteralType.Double,
                Value = 2.3
            });

            string result = this.Serialize(node, node.GetType());
            Console.WriteLine(result);
        }

        [Test]
        public void Binary()
        {
            var binary = new ASTBinaryExpression()
            {
                Left = new ASTBinaryExpression()
                {
                    Left = new ASTLiteral()
                    {
                        LiteralType = LiteralType.Double,
                        Value = 10.56d
                    },
                    Operator = BinaryOperatorType.Add,
                    Right = new ASTLiteral()
                    {
                        LiteralType = LiteralType.Double,
                        Value = 3.21d
                    }
                },
                Operator = BinaryOperatorType.Divide,
                Right = new ASTLiteral()
                {
                    LiteralType = LiteralType.Double,
                    Value = 2d
                }
            };

            string result = this.Serialize(binary, binary.GetType());
            Console.WriteLine(result);
        }

        [Test]
        public void Literal()
        {
            ASTLiteral literal = new ASTLiteral();
            literal.LiteralType = LiteralType.Double;
            literal.Value = 10.5d;


            string result = this.Serialize(literal, literal.GetType());
            Console.WriteLine(result);

        }

        private string Serialize(ASTAnyNode node, Type type)
        {
            String result;

            var serializer = new DataContractSerializer(node.GetType());
            var settings = new XmlWriterSettings
            {
                Indent = true,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,

            };
            using (var sw = new StringWriter())
            {
                using (var xw = XmlWriter.Create(sw, settings))
                {
                    serializer.WriteStartObject(xw, node);
                    xw.WriteAttributeString("xmlns", "xs", null, "http://www.w3.org/2001/XMLSchema");
                    serializer.WriteObjectContent(xw, node);
                    serializer.WriteEndObject(xw);
                    //serializer.WriteObject(xw, node);
                }
                result = sw.ToString();
            }

            return result;
        }

        private ASTAnyNode RunModel(string input)
        {
            Parser_v2 parser = new Parser_v2(new Scanner(new StringCharSource(input)));
            ASTModel model = parser.Model;
            return model;
        }
    }
}
