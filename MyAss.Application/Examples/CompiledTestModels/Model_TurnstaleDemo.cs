using MyAss.Framework_v2;
using MyAss.Framework_v2.BuiltIn.Blocks;
using MyAss.Framework_v2.BuiltIn.Commands;
using MyAss.Framework_v2.Commands;
using MyAss.Framework_v2.OperandTypes;
using System;

namespace Modeling
{
    public class TheModel : AbstractModel
    {
        private ReferencedNumber Server;
        private ReferencedNumber Turn;

        public TheModel()
        {
            this.Server = 10000;
            base.AddName("Server", this.Server);
            TheModel theModel = this;
            Storage storage = new Storage(new ParExpression(new ExpressionDelegate(theModel.Verb1_Operand1)));
            storage.SetId(this.Server);
            base.AddVerb(storage);

            TheModel theModel1 = this;
            Start start = new Start(new ParExpression(new ExpressionDelegate(theModel1.Verb2_Operand1)), null, null, null);
            base.AddVerb(start);

            TheModel theModel2 = this;
            TheModel theModel3 = this;
            Generate generate = new Generate(new ParExpression(new ExpressionDelegate(theModel2.Verb3_Operand1)), new ParExpression(new ExpressionDelegate(theModel3.Verb3_Operand2)), null, null, null);
            base.AddVerb(generate);

            this.Turn = 10001;
            base.AddName("Turn", this.Turn);
            TheModel theModel4 = this;
            Queue queue = new Queue(new ParExpression(new ExpressionDelegate(theModel4.Verb4_Operand1)), null);
            base.AddVerb(queue);

            TheModel theModel5 = this;
            Enter enter = new Enter(new ParExpression(new ExpressionDelegate(theModel5.Verb5_Operand1)), null);
            base.AddVerb(enter);

            TheModel theModel6 = this;
            Depart depart = new Depart(new ParExpression(new ExpressionDelegate(theModel6.Verb6_Operand1)), null);
            base.AddVerb(depart);

            TheModel theModel7 = this;
            TheModel theModel8 = this;
            Advance advance = new Advance(new ParExpression(new ExpressionDelegate(theModel7.Verb7_Operand1)), new ParExpression(new ExpressionDelegate(theModel8.Verb7_Operand2)));
            base.AddVerb(advance);

            TheModel theModel9 = this;
            Leave leave = new Leave(new ParExpression(new ExpressionDelegate(theModel9.Verb8_Operand1)), null);
            base.AddVerb(leave);

            TheModel theModel10 = this;
            Terminate terminate = new Terminate(new ParExpression(new ExpressionDelegate(theModel10.Verb9_Operand1)));
            base.AddVerb(terminate);
        }

        public virtual double Verb1_Operand1()
        {
            return 1;
        }

        public virtual double Verb2_Operand1()
        {
            return 10000;
        }

        public virtual double Verb3_Operand1()
        {
            return 7;
        }

        public virtual double Verb3_Operand2()
        {
            return 5;
        }

        public virtual double Verb4_Operand1()
        {
            return (double)this.Turn;
        }

        public virtual double Verb5_Operand1()
        {
            return (double)this.Server;
        }

        public virtual double Verb6_Operand1()
        {
            return (double)this.Turn;
        }

        public virtual double Verb7_Operand1()
        {
            return 5;
        }

        public virtual double Verb7_Operand2()
        {
            return 3;
        }

        public virtual double Verb8_Operand1()
        {
            return (double)this.Server;
        }

        public virtual double Verb9_Operand1()
        {
            return 1;
        }
    }
}

