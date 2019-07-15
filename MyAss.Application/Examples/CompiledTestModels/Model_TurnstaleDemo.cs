using MyAss.Framework;
using MyAss.Framework.BuiltIn.Blocks;
using MyAss.Framework.BuiltIn.Commands;
using MyAss.Framework.Commands;
using MyAss.Framework.OperandTypes;
using System;
using MyAss.Utilities.Reports;

namespace MyAss.Application.Examples.CompiledTestModels.Turnstale
{
    public class TheModel : AbstractModel
    {
        private ReferencedNumber Server;
        private ReferencedNumber Turn;

        public TheModel()
        {
            this.Server = 10000;
            base.AddName("Server", this.Server);
            Storage storage = new Storage(new NumberOperand(1));
            storage.SetId(this.Server);
            base.AddVerb(storage);

            Start start = new Start(new NumberOperand(10000), null, null, null);
            base.AddVerb(start);

            Generate generate = new Generate(new NumberOperand(7), new NumberOperand(5), null, null, null);
            base.AddVerb(generate);

            this.Turn = 10001;
            base.AddName("Turn", this.Turn);
            TheModel theModel = this;
            Queue queue = new Queue(new ParExpression(new ExpressionDelegate(theModel.Verb4_Operand1)), null);
            base.AddVerb(queue);

            TheModel theModel1 = this;
            Enter enter = new Enter(new ParExpression(new ExpressionDelegate(theModel1.Verb5_Operand1)), null);
            base.AddVerb(enter);

            TheModel theModel2 = this;
            Depart depart = new Depart(new ParExpression(new ExpressionDelegate(theModel2.Verb6_Operand1)), null);
            base.AddVerb(depart);

            Advance advance = new Advance(new NumberOperand(5), new NumberOperand(3));
            base.AddVerb(advance);

            TheModel theModel3 = this;
            Leave leave = new Leave(new ParExpression(new ExpressionDelegate(theModel3.Verb8_Operand1)), null);
            base.AddVerb(leave);

            Terminate terminate = new Terminate(new NumberOperand(1));
            base.AddVerb(terminate);
        }

        public double Verb4_Operand1()
        {
            return (double)this.Turn;
        }

        public double Verb5_Operand1()
        {
            return (double)this.Server;
        }

        public double Verb6_Operand1()
        {
            return (double)this.Turn;
        }

        public double Verb8_Operand1()
        {
            return (double)this.Server;
        }
    }

    public class Program
    {
        public static void _Main()
        {
            Simulation simulation = new Simulation(new TheModel());
            StandardReport.PrintReport(simulation);
            Console.WriteLine();
        }
    }
}
