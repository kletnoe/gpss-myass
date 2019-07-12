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
using MyAss.Compiler.AST;
using NUnit.Framework;

namespace MyAss.Compiler.Tests.ASTTests
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
            string result = node.Serialize();
            Console.WriteLine(result);
        }

        [Test]
        public void Model()
        {
            ASTModel node = new ASTModel();
            node.Verbs.Add(new ASTVerb());
            node.Verbs.Add(new ASTVerb());

            string result = node.Serialize();
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

            string result = node.Serialize();
            Console.WriteLine(result);
        }

        [Test]
        public void Binary()
        {
            var node = new ASTBinaryExpression()
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

            string result = node.Serialize();
            Console.WriteLine(result);
        }

        [Test]
        public void Literal()
        {
            ASTLiteral node = new ASTLiteral();
            node.LiteralType = LiteralType.Double;
            node.Value = 10.5d;


            string result = node.Serialize();
            Console.WriteLine(result);

        }

        private ASTAnyNode RunModel(string input)
        {
            Parser parser = new Parser(new Scanner(new CharSourceTokenizer(input)));
            ASTModel model = parser.Model;
            return model;
        }
    }
}
