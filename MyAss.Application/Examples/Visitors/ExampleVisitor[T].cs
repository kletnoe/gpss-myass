using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Application.Examples.Visitors
{
    public class ExampleVisitor_T_
    {
        public interface IVisitableNode
        {
            T Accept<T>(IVisitor<T> visitor);
        }

        public interface IVisitor<T>
        {
            T Visit(Root node);
            T Visit(Binary node);
            T Visit(Literal node);
        }

        public class Root : IVisitableNode
        {
            public Binary Node { get; set; }

            public T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Binary : IVisitableNode
        {
            public Literal Left { get; set; }
            public string Operator { get; set; }
            public Literal Right { get; set; }

            public T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class Literal : IVisitableNode
        {
            public int Value { get; set; }

            public T Accept<T>(IVisitor<T> visitor)
            {
                return visitor.Visit(this);
            }
        }

        public class CodeVisitor : IVisitor<CodeObject>
        {
            public CodeObject Visit(Root node)
            {
                CodeAssignStatement stat = new CodeAssignStatement();
                stat.Left = new CodeVariableReferenceExpression("result");
                stat.Right = (CodeExpression)node.Node.Accept(this);
                return stat;
            }

            public CodeObject Visit(Binary node)
            {
                CodeBinaryOperatorExpression expr = new CodeBinaryOperatorExpression();
                expr.Left = (CodeExpression)node.Left.Accept(this);
                expr.Operator = CodeBinaryOperatorType.Add;
                expr.Right = (CodeExpression)node.Right.Accept(this);
                return expr;
            }

            public CodeObject Visit(Literal node)
            {
                return new CodePrimitiveExpression(node.Value);
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

            IVisitor<CodeObject> vis = new CodeVisitor();
            var result = vis.Visit(root);

            Console.WriteLine();
        }
    }
}
