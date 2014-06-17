using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssApplication.Examples.Visitors
{
    public class ExampleVisitor
    {
        public interface IVisitableNode
        {
            void Accept(IVisitor visitor);
        }

        public interface IVisitor
        {
            void Visit(Root visited);
            void Visit(Binary visited);
            void Visit(Literal visited);
        }

        public class Root : IVisitableNode
        {
            public Binary Node { get; set; }

            public void Accept(IVisitor visitor)
            {
                visitor.Visit(this);
            }
        }

        public class Binary : IVisitableNode
        {
            public Literal Left { get; set; }
            public string Operator { get; set; }
            public Literal Right { get; set; }

            public void Accept(IVisitor visitor)
            {
                throw new NotImplementedException();
            }
        }

        public class Literal : IVisitableNode
        {
            public int Value { get; set; }

            public void Accept(IVisitor visitor)
            {
                throw new NotImplementedException();
            }
        }

        public class CodeVisitor : IVisitor
        {
            public void Visit(Root node)
            {

            }

            public void Visit(Binary node)
            {

            }

            public void Visit(Literal node)
            {

            }
        }


        public static void Run()
        {
            Root root = new Root()
            {
                Node = new Binary()
                {
                    Left = new Literal()
                    {
                        Value = 10
                    },
                    Operator = "+",
                    Right = new Literal()
                    {
                        Value = 15
                    }
                }
            };

            IVisitor vis = new CodeVisitor();
            vis.Visit(root);

            Console.WriteLine();
        }

    }
}
