using System;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;
using MyAss.Framework.Procedures;
using MyAss.Framework_v2;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.BuiltIn.Blocks;
using MyAss.Framework_v2.BuiltIn.Commands;
using MyAss.Framework_v2.BuiltIn.SNA;
using MyAss.Framework_v2.Commands;
using MyAss.Framework_v2.Entities;
using MyAss.Framework_v2.OperandTypes;

namespace TestModels
{
    class MM3Model_Dynamic : AbstractModel
    {
        private int SERVER;
        private int GENERATECOUNTER;
        private int TAIL;
        private int GOAWAY;
        private int REJETIONPROB;
        private int REJECTCOUNTER;

        public MM3Model_Dynamic()
        {
            // Server STORAGE 3
            this.SERVER = 10000;
            this.AddName("SERVER", SERVER);
            ICommand command1 = new Storage(new ParExpression(this.Command1_Operand1));
            command1.SetId(this.SERVER);
            this.AddVerb(command1);

            // START 1000
            ICommand command2 = new Start(new ParExpression(this.Command2_Operand1), null, null, null);
            this.AddVerb(command2);

            // GENERATE (Exponential(1,0,1/2))
            IBlock block1 = new Generate(new ParExpression(this.Block1_Operand1), null, null, null, null);
            this.AddVerb(block1);

            // SAVEVALUE GenerateCounter,X$GenerateCounter+1
            this.GENERATECOUNTER = 10001;
            this.AddName("GENERATECOUNTER", GENERATECOUNTER);
            IBlock block2 = new Savevalue(new ParExpression(this.Block2_Operand1), new ParExpression(this.Block2_Operand2));
            this.AddVerb(block2);

            // TEST L Q$Tail,20,GoAway		;Jump if in Queue >20
            this.TAIL = 10002;
            this.AddName("TAIL", TAIL);
            this.GOAWAY = 10003;
            this.AddName("GOAWAY", GOAWAY);
            IBlock block3 = new Test(new LiteralOperand("L"), new ParExpression(this.Block3_Operand2), new ParExpression(this.Block3_Operand3), new ParExpression(this.Block3_Operand4));
            this.AddVerb(block3);

            // QUEUE Tail
            IBlock block4 = new Queue(new ParExpression(this.Block4_Operand1), null);
            this.AddVerb(block4);

            // ENTER Server,1
            IBlock block5 = new Enter(new ParExpression(this.Block5_Operand1), new ParExpression(this.Block5_Operand2));
            this.AddVerb(block5);

            // DEPART Tail
            IBlock block6 = new Depart(new ParExpression(this.Block6_Operand1), null);
            this.AddVerb(block6);

            // ADVANCE (Exponential(2,0,1/0.2))
            IBlock block7 = new Advance(new ParExpression(this.Block7_Operand1), null);
            this.AddVerb(block7);

            // LEAVE Server,1
            IBlock block8 = new Leave(new ParExpression(this.Block8_Operand1), new ParExpression(this.Block8_Operand2));
            this.AddVerb(block8);

            // SAVEVALUE RejetionProb,(X$RejectCounter/X$GenerateCounter)
            this.REJETIONPROB = 10004;
            this.AddName("REJETIONPROB", REJETIONPROB);
            this.REJECTCOUNTER = 10005;
            this.AddName("REJECTCOUNTER", REJECTCOUNTER);
            IBlock block9 = new Savevalue(new ParExpression(this.Block9_Operand1), new ParExpression(this.Block9_Operand2));
            this.AddVerb(block9);

            // TERMINATE 1
            IBlock block10 = new Terminate(new ParExpression(this.Block10_Operand1));
            this.AddVerb(block10);

            // GoAway	SAVEVALUE RejectCounter,X$RejectCounter+1
            this.GOAWAY = 11;
            this.ReplaceNameId("GOAWAY", GOAWAY);
            IBlock block11 = new Savevalue(new ParExpression(this.Block11_Operand1), new ParExpression(this.Block11_Operand2));
            this.AddVerb(block11);

            // TERMINATE 		;Delete rejected.
            IBlock block12 = new Terminate(null);
            this.AddVerb(block12);
        }

        public double Command1_Operand1() { return 3; }
        public double Command2_Operand1() { return 10000; }

        public double Block1_Operand1()
        {
            return Distributions.Exponential(1, 0, 1.0 / 2.0);
        }

        public double Block2_Operand1() { return this.GENERATECOUNTER; }

        public double Block2_Operand2()
        {
            return this.X(this.GENERATECOUNTER) + 1;
        }

        public double Block3_Operand2()
        {
            return this.Q(this.TAIL);
        }

        public double Block3_Operand3() { return 20; }
        public double Block3_Operand4() { return this.GOAWAY; }

        public double Block4_Operand1() { return this.TAIL; }

        public double Block5_Operand1() { return this.SERVER; }
        public double Block5_Operand2() { return 1; }

        public double Block6_Operand1() { return this.TAIL; }

        public double Block7_Operand1()
        {
            return Distributions.Exponential(2, 0, 1.0 / 0.2);
        }

        public double Block8_Operand1() { return this.SERVER; }
        public double Block8_Operand2() { return 1; }

        public double Block9_Operand1() { return this.REJETIONPROB; }

        public double Block9_Operand2()
        {
            double rej = this.X(this.REJECTCOUNTER);
            double gen = this.X(this.GENERATECOUNTER);

            return rej/gen;
        }

        public double Block10_Operand1() { return 1; }

        public double Block11_Operand1() { return this.REJECTCOUNTER; }

        public double Block11_Operand2()
        {
            return this.X(this.REJECTCOUNTER) + 1;
        }


        public double Q(int entityId)
        {
            try
            {
                IEntity entity = simulation.GetEntity(entityId);
                dynamic dynamicEntity = entity;
                return (double)dynamicEntity.Q;
            }
            catch (RuntimeBinderException)
            {
                return 0;
            }
            catch (KeyNotFoundException)
            {
                return 0;
            }
        }

        public double X(int entityId)
        {
            try
            {
                IEntity entity = simulation.GetEntity(entityId);
                dynamic dynamicEntity = entity;
                return (double)dynamicEntity.X;
            }
            catch (RuntimeBinderException)
            {
                return 0;
            }
            catch (KeyNotFoundException)
            {
                return 0;
            }
        }
    }
}
