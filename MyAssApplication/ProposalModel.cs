using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework;
using MyAssFramework.Blocks;
using MyAssFramework.OperandTypes_Test;
using MyAssFramework.RelationalOp;
using MyAssUtilities.Reports;
using MyAssFramework.Entities;

namespace MyAssApplication
{
    class ProposalModel
    {
        private Simulation simulation = new Simulation();

        private void DefineBlocks()
        {
            Operand block1_operand1 = new ParExpression(Expressions.Block1_Operand1);
            Operand block1_operand2 = null;
            Operand block1_operand3 = null;
            Operand block1_operand4 = null;
            Operand block1_operand5 = null;

            IBlock block1 = new Generate(block1_operand1, block1_operand2, block1_operand3, block1_operand4, block1_operand5);
            IBlock block2 = new Savevalue(new Number(2), Savevalue.Operation.Plus, new Number(1));
            IBlock block3 = new Test(RelationalOperator.LT, new SNA_Q(0), new Number(20), new Number(11));
            IBlock block4 = new Queue(new Number(0), new Number(1));
            IBlock block5 = new Enter(new Number(4), new Number(1));
            IBlock block6 = new Depart(new Number(0), new Number(1));
            IBlock block7 = new Advance(new ParExpression(Expressions.Block7_Operand1), null);
            IBlock block8 = new Leave(new Number(4), new Number(1));
            IBlock block9 = new Savevalue(new Number(3), Savevalue.Operation.Set, new ParExpression(Expressions.Block9_Operand2));
            IBlock block10 = new Terminate(new Number(1));

            IBlock block11 = new Savevalue(new Number(1), Savevalue.Operation.Plus, new Number(1));
            IBlock block12 = new Terminate(new Number(0));

            this.simulation.Blocks.Add(block1);
        }

        private void DefineEntities()
        {
            IEntity entity1 = new QueueEntity(0);
            IEntity entity2 = new SavevalueEntity(1, "0");
            IEntity entity3 = new SavevalueEntity(2, "0");
            IEntity entity4 = new SavevalueEntity(3, "0");
            IEntity entity5 = new StorageEntity(4, 1);
        } 
    }

    static class Expressions{
        public static double Block1_Operand1()
        {
            return 10;
            //return Exponential.GetNext(0, 0, 2);
        }

        public static double Block7_Operand1()
        {
            return 30;
            //return Exponential.GetNext(0, 0, 10);
        }

        public static double Block9_Operand2()
        {
            SNA_X rej = new SNA_X(1);
            SNA_X gen = new SNA_X(2);

            return rej.GetValue() / gen.GetValue();
        }

    }
}
