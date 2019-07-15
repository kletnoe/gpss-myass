using MyAss.Framework;
using MyAss.Framework.Blocks;
using MyAss.Framework.BuiltIn.Blocks;
using MyAss.Framework.BuiltIn.Commands;
using MyAss.Framework.BuiltIn.Procedures;
using MyAss.Framework.BuiltIn.SNA;
using MyAss.Framework.Commands;
using MyAss.Framework.OperandTypes;

namespace MyAss.Application.Examples.CompiledTestModels
{
    class MM3Model : AbstractModel
    {
        private ReferencedNumber SERVER;
        private ReferencedNumber GENERATECOUNTER;
        private ReferencedNumber TAIL;
        private ReferencedNumber GOAWAY;
        private ReferencedNumber REJETIONPROB;
        private ReferencedNumber REJECTCOUNTER;

        public MM3Model()
        {
            // Server STORAGE 3
            this.SERVER = 10000;
            this.AddName("SERVER", SERVER);
            AnyCommand command1 = new Storage(new ParExpression(this.Command1_Operand1));
            command1.SetId(this.SERVER);
            this.AddVerb(command1);

            // START 1000
            AnyCommand command2 = new Start(new ParExpression(this.Command2_Operand1), null, null, null);
            this.AddVerb(command2);

            // GENERATE (Exponential(1,0,1/2))
            AnyBlock block1 = new Generate(new ParExpression(this.Block1_Operand1), null, null, null, null);
            this.AddVerb(block1);

            // SAVEVALUE GenerateCounter,X$GenerateCounter+1
            this.GENERATECOUNTER = 10001;
            this.AddName("GENERATECOUNTER", GENERATECOUNTER);
            AnyBlock block2 = new Savevalue(new ParExpression(this.Block2_Operand1), new ParExpression(this.Block2_Operand2));
            this.AddVerb(block2);

            // TEST L Q$Tail,20,GoAway		;Jump if in Queue >20
            this.TAIL = 10002;
            this.AddName("TAIL", TAIL);
            this.GOAWAY = 10003;
            this.AddName("GOAWAY", GOAWAY);
            AnyBlock block3 = new Test(new LiteralOperand("L"), new ParExpression(this.Block3_Operand2), new ParExpression(this.Block3_Operand3), new ParExpression(this.Block3_Operand4));
            this.AddVerb(block3);

            // QUEUE Tail
            AnyBlock block4 = new Queue(new ParExpression(this.Block4_Operand1), null);
            this.AddVerb(block4);

            // ENTER Server,1
            AnyBlock block5 = new Enter(new ParExpression(this.Block5_Operand1), new ParExpression(this.Block5_Operand2));
            this.AddVerb(block5);

            // DEPART Tail
            AnyBlock block6 = new Depart(new ParExpression(this.Block6_Operand1), null);
            this.AddVerb(block6);

            // ADVANCE (Exponential(2,0,1/0.2))
            AnyBlock block7 = new Advance(new ParExpression(this.Block7_Operand1), null);
            this.AddVerb(block7);

            // LEAVE Server,1
            AnyBlock block8 = new Leave(new ParExpression(this.Block8_Operand1), new ParExpression(this.Block8_Operand2));
            this.AddVerb(block8);

            // SAVEVALUE RejetionProb,(X$RejectCounter/X$GenerateCounter)
            this.REJETIONPROB = 10004;
            this.AddName("REJETIONPROB", REJETIONPROB);
            this.REJECTCOUNTER = 10005;
            this.AddName("REJECTCOUNTER", REJECTCOUNTER);
            AnyBlock block9 = new Savevalue(new ParExpression(this.Block9_Operand1), new ParExpression(this.Block9_Operand2));
            this.AddVerb(block9);

            // TERMINATE 1
            AnyBlock block10 = new Terminate(new ParExpression(this.Block10_Operand1));
            this.AddVerb(block10);

            // GoAway	SAVEVALUE RejectCounter,X$RejectCounter+1
            this.GOAWAY = 11;
            this.ReplaceNameId("GOAWAY", GOAWAY);
            AnyBlock block11 = new Savevalue(new ParExpression(this.Block11_Operand1), new ParExpression(this.Block11_Operand2));
            this.AddVerb(block11);

            // TERMINATE 		;Delete rejected.
            AnyBlock block12 = new Terminate(null);
            this.AddVerb(block12);
        }

        public double Command1_Operand1() { return 3; }
        public double Command2_Operand1() { return 100001; }

        public double Block1_Operand1()
        {
            return Distributions.Exponential(1, 0, 1.0 / 2.0);
        }

        public double Block2_Operand1() { return this.GENERATECOUNTER; }

        public double Block2_Operand2()
        {
            return SavevalueSNA.X(this.simulation, this.GENERATECOUNTER) + 1;
        }

        public double Block3_Operand2()
        {
            return QueueSNA.Q(this.simulation, this.TAIL);
        }

        public double Block3_Operand3() { return 20; }
        public double Block3_Operand4() { return this.GOAWAY; }

        public double Block4_Operand1() { return this.TAIL; }

        public double Block5_Operand1() { return this.SERVER; }
        public double Block5_Operand2() { return 1; }

        public double Block6_Operand1() { return this.TAIL; }

        public double Block7_Operand1()
        {
            return Distributions.Exponential(1, 0, 1.0 / 0.2);
        }

        public double Block8_Operand1() { return this.SERVER; }
        public double Block8_Operand2() { return 1; }

        public double Block9_Operand1() { return this.REJETIONPROB; }

        public double Block9_Operand2()
        {
            double rej = SavevalueSNA.X(this.simulation, this.REJECTCOUNTER);
            double gen = SavevalueSNA.X(this.simulation, this.GENERATECOUNTER);

            return rej/gen;
        }

        public double Block10_Operand1() { return 1; }

        public double Block11_Operand1() { return this.REJECTCOUNTER; }

        public double Block11_Operand2()
        {
            return SavevalueSNA.X(this.simulation, this.REJECTCOUNTER) + 1;
        }
    }
}
