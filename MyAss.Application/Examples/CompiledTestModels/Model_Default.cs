using MyAss.Framework.Procedures;
using MyAss.Framework_v2;
using MyAss.Framework_v2.BuiltIn.Blocks;
using MyAss.Framework_v2.BuiltIn.Commands;
using MyAss.Framework_v2.BuiltIn.SNA;
using MyAss.Framework_v2.Commands;
using MyAss.Framework_v2.OperandTypes;
using System;

namespace Modeling_
{
    public class TheModel : AbstractModel
    {
        private ReferencedNumber Server;

        private ReferencedNumber GenerateCounter;

        private ReferencedNumber Tail;

        private ReferencedNumber GoAway;

        private ReferencedNumber RejetionProb;

        private ReferencedNumber RejectCounter;

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
            Generate generate = new Generate(new ParExpression(new ExpressionDelegate(theModel2.Verb3_Operand1)), null, null, null, null);
            base.AddVerb(generate);
            this.GenerateCounter = 10001;
            base.AddName("GenerateCounter", this.GenerateCounter);
            TheModel theModel3 = this;
            TheModel theModel4 = this;
            Savevalue savevalue = new Savevalue(new ParExpression(new ExpressionDelegate(theModel3.Verb4_Operand1)), new ParExpression(new ExpressionDelegate(theModel4.Verb4_Operand2)));
            base.AddVerb(savevalue);
            this.Tail = 10002;
            base.AddName("Tail", this.Tail);
            this.GoAway = 10003;
            base.AddName("GoAway", this.GoAway);
            LiteralOperand literalOperand = new LiteralOperand("L");
            TheModel theModel5 = this;
            ParExpression parExpression = new ParExpression(new ExpressionDelegate(theModel5.Verb5_Operand2));
            TheModel theModel6 = this;
            TheModel theModel7 = this;
            Test test = new Test(literalOperand, parExpression, new ParExpression(new ExpressionDelegate(theModel6.Verb5_Operand3)), new ParExpression(new ExpressionDelegate(theModel7.Verb5_Operand4)));
            base.AddVerb(test);
            TheModel theModel8 = this;
            Queue queue = new Queue(new ParExpression(new ExpressionDelegate(theModel8.Verb6_Operand1)), null);
            base.AddVerb(queue);
            TheModel theModel9 = this;
            TheModel theModel10 = this;
            Enter enter = new Enter(new ParExpression(new ExpressionDelegate(theModel9.Verb7_Operand1)), new ParExpression(new ExpressionDelegate(theModel10.Verb7_Operand2)));
            base.AddVerb(enter);
            TheModel theModel11 = this;
            Depart depart = new Depart(new ParExpression(new ExpressionDelegate(theModel11.Verb8_Operand1)), null);
            base.AddVerb(depart);
            TheModel theModel12 = this;
            Advance advance = new Advance(new ParExpression(new ExpressionDelegate(theModel12.Verb9_Operand1)), null);
            base.AddVerb(advance);
            TheModel theModel13 = this;
            TheModel theModel14 = this;
            Leave leave = new Leave(new ParExpression(new ExpressionDelegate(theModel13.Verb10_Operand1)), new ParExpression(new ExpressionDelegate(theModel14.Verb10_Operand2)));
            base.AddVerb(leave);
            this.RejetionProb = 10004;
            base.AddName("RejetionProb", this.RejetionProb);
            this.RejectCounter = 10005;
            base.AddName("RejectCounter", this.RejectCounter);
            TheModel theModel15 = this;
            TheModel theModel16 = this;
            Savevalue savevalue1 = new Savevalue(new ParExpression(new ExpressionDelegate(theModel15.Verb11_Operand1)), new ParExpression(new ExpressionDelegate(theModel16.Verb11_Operand2)));
            base.AddVerb(savevalue1);
            TheModel theModel17 = this;
            Terminate terminate = new Terminate(new ParExpression(new ExpressionDelegate(theModel17.Verb12_Operand1)));
            base.AddVerb(terminate);
            this.GoAway = 11;
            base.ReplaceNameId("GoAway", this.GoAway);
            TheModel theModel18 = this;
            TheModel theModel19 = this;
            Savevalue savevalue2 = new Savevalue(new ParExpression(new ExpressionDelegate(theModel18.Verb13_Operand1)), new ParExpression(new ExpressionDelegate(theModel19.Verb13_Operand2)));
            base.AddVerb(savevalue2);
            base.AddVerb(new Terminate(null));
        }

        public virtual double Verb1_Operand1()
        {
            return 3;
        }

        public virtual double Verb10_Operand1()
        {
            return (double)this.Server;
        }

        public virtual double Verb10_Operand2()
        {
            return 1;
        }

        public virtual double Verb11_Operand1()
        {
            return (double)this.RejetionProb;
        }

        public virtual double Verb11_Operand2()
        {
            double num = SavevalueSNA.X(this.simulation, this.RejectCounter) / SavevalueSNA.X(this.simulation, this.GenerateCounter);
            return num;
        }

        public virtual double Verb12_Operand1()
        {
            return 1;
        }

        public virtual double Verb13_Operand1()
        {
            return (double)this.RejectCounter;
        }

        public virtual double Verb13_Operand2()
        {
            double num = SavevalueSNA.X(this.simulation, this.RejectCounter) + 1;
            return num;
        }

        public virtual double Verb2_Operand1()
        {
            return 10000;
        }

        public virtual double Verb3_Operand1()
        {
            return Distributions.Exponential(1, 0, 0.5);
        }

        public virtual double Verb4_Operand1()
        {
            return (double)this.GenerateCounter;
        }

        public virtual double Verb4_Operand2()
        {
            double num = SavevalueSNA.X(this.simulation, this.GenerateCounter) + 1;
            return num;
        }

        public virtual double Verb5_Operand2()
        {
            return QueueSNA.Q(this.simulation, this.Tail);
        }

        public virtual double Verb5_Operand3()
        {
            return 20;
        }

        public virtual double Verb5_Operand4()
        {
            return (double)this.GoAway;
        }

        public virtual double Verb6_Operand1()
        {
            return (double)this.Tail;
        }

        public virtual double Verb7_Operand1()
        {
            return (double)this.Server;
        }

        public virtual double Verb7_Operand2()
        {
            return 1;
        }

        public virtual double Verb8_Operand1()
        {
            return (double)this.Tail;
        }

        public virtual double Verb9_Operand1()
        {
            return Distributions.Exponential(2, 0, 5);
        }
    }
}