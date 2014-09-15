using MyAss.Framework.Procedures;
using MyAss.Framework_v2;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.BuiltIn.Blocks;
using MyAss.Framework_v2.BuiltIn.Commands;
using MyAss.Framework_v2.BuiltIn.SNA;
using MyAss.Framework_v2.Commands;
using MyAss.Framework_v2.OperandTypes;

namespace MyAss.Application.Examples.TestModels
{
    class MM3Model : AbstractModel
    {
        // TODO: Figure out how to create Block labels binding =/

        public override AbstractModel Construct()
        {
            // Add names
            this.AddName(10001, "GENERATECOUNTER");
            this.AddName(11, "GOAWAY");
            this.AddName(10004, "REJECTCOUNTER");
            this.AddName(10003, "REJETIONPROB");
            this.AddName(10000, "SERVER");
            this.AddName(10002, "TAIL");


            ICommand command1 = new Storage(new Number(3));
            command1.SetId(10000);
            this.AddVerb(command1);
            ICommand command2 = new Start(new Number(1000), null, null, null);
            this.AddVerb(command2);

            IBlock block1 = new Generate(new ParExpression(new ExpressionDelegate(this.Block1_Operand1)), null, null, null, null);
            this.AddVerb(block1);
            IBlock block2 = new Savevalue(new Number(10001), new ParExpression(this.Block2_Operand2));
            this.AddVerb(block2);
            IBlock block3 = new Test(new LiteralOperand("L"), new ParExpression(this.Block3_Operand1), new Number(20), new Number(11));
            this.AddVerb(block3);
            IBlock block4 = new Queue(new Number(10002), new Number(1));
            this.AddVerb(block4);
            IBlock block5 = new Enter(new Number(10000), new Number(1));
            this.AddVerb(block5);
            IBlock block6 = new Depart(new Number(10002), new Number(1));
            this.AddVerb(block6);
            IBlock block7 = new Advance(new ParExpression(this.Block7_Operand1), null);
            this.AddVerb(block7);
            IBlock block8 = new Leave(new Number(10000), new Number(1));
            this.AddVerb(block8);
            IBlock block9 = new Savevalue(new Number(10003), new ParExpression(this.Block9_Operand2));
            this.AddVerb(block9);
            IBlock block10 = new Terminate(new Number(1));
            this.AddVerb(block10);

            IBlock block11 = new Savevalue(new Number(10004), new ParExpression(this.Block11_Operand2));
            //block11.SetLabel(11);
            this.AddVerb(block11);
            IBlock block12 = new Terminate(new Number(0));
            this.AddVerb(block12);

            return this;
        }

        public double Block1_Operand1()
        {
            return Distributions.Exponential(1, 0, 1.0 / 2.0);
        }

        public double Block2_Operand2()
        {
            return SavevalueSNA.X(this.Simulation, 10001) + 1;
        }

        public double Block3_Operand1()
        {
            return QueueSNA.Q(this.Simulation, 10002);
        }

        public double Block7_Operand1()
        {
            return Distributions.Exponential(1, 0, 1.0 / 0.2);
        }

        public double Block9_Operand2()
        {
            double rej = SavevalueSNA.X(this.Simulation, 10004);
            double gen = SavevalueSNA.X(this.Simulation, 10001);

            return rej/gen;
        }

        public double Block11_Operand2()
        {
            return SavevalueSNA.X(this.Simulation, 10004) + 1;
        }
    }
}
