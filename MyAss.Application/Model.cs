using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Framework;
using MyAss.Framework.Blocks;
using MyAss.Framework.OperandTypes_Test;
using MyAss.Framework.RelationalOp;
using MyAss.Utilities.Reports;

namespace MyAss.Application
{
    public static class Model
    {
        public static void RunDefaultModel()
        {
            Simulation.It.Entities = new List<MyAss.Framework.Entities.IEntity>()
            {
                new MyAss.Framework.Entities.QueueEntity(0),
                new MyAss.Framework.Entities.SavevalueEntity(1, "0"),
                new MyAss.Framework.Entities.SavevalueEntity(2, "0"),
                new MyAss.Framework.Entities.SavevalueEntity(3, "0"),
                new MyAss.Framework.Entities.StorageEntity(4, 1)
            };

            Simulation.It.Blocks = new List<IBlock>()
            {
                new Generate(new ParExpression(GetGenerateOperand), null, null, null, null),
                new Savevalue(new Number(2), Savevalue.Operation.Plus, new Number(1)),
                new Test(RelationalOperator.LT, new SNA_Q(0), new Number(20), new Number(11)),
                new Queue(new Number(0), new Number(1)),
                new Enter(new Number(4), new Number(1)),
                new Depart(new Number(0), new Number(1)),
                new Advance(new ParExpression(GetAdvanceOperand), null),
                new Leave(new Number(4), new Number(1)),
                new Savevalue(new Number(3), Savevalue.Operation.Set, new ParExpression(GetRejectionProb)),
                new Terminate(new Number(1)),

                new Savevalue(new Number(1), Savevalue.Operation.Plus, new Number(1)),
                new Terminate(new Number(0))
            };

            DateTime now = DateTime.Now;

            Simulation.It.Start(10);

            StandardReport.PrintReport(Simulation.It);

            Console.WriteLine(DateTime.Now - now);
        }


        public static void Test(IDoubleOperand op)
        {
            Console.WriteLine(op);
        }


        public static double GetRejectionProb()
        {
            SNA_X rej = new SNA_X(1);
            SNA_X gen = new SNA_X(2);

            return rej.GetValue() / gen.GetValue();
        }

        public static double GetGenerateOperand()
        {
            return 10;
            //return Exponential.GetNext(0, 0, 2);
        }

        public static double GetAdvanceOperand()
        {
            return 30;
            //return Exponential.GetNext(0, 0, 10);
        }
    }
}
