using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssFramework;
using MyAssFramework.Blocks;
using MyAssFramework.RelationalOp;
using MyAssFramework.OperandTypes_Test;
using MyAssFramework.Distributions;
using MyAssFramework.SNA;
using System.Linq.Expressions;
using MyAssUtilities.Reports;

namespace MyAssApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Simulation.It.Entities = new List<MyAssFramework.Entities.IEntity>()
            {
                new MyAssFramework.Entities.QueueEntity(0),
                new MyAssFramework.Entities.SavevalueEntity(1, "0"),
                new MyAssFramework.Entities.SavevalueEntity(2, "0"),
                new MyAssFramework.Entities.SavevalueEntity(3, "0"),
                new MyAssFramework.Entities.StorageEntity(4, 3)
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

            Simulation.It.Start(1000);

            StandardReport.PrintReport(Simulation.It);

            Console.WriteLine(DateTime.Now - now);
        }


        public static void Test(Operand op)
        {
            Console.WriteLine(op);
        }


        public static double GetRejectionProb()
        {
            SNA_X rej = new SNA_X(1);
            SNA_X gen = new SNA_X(2);

            return rej.GetValue() / gen.GetValue();
        }

        public static double GetDouble()
        {
            return new Uniform(0, 7, 13).GetNext();
        }

        public static double GetGenerateOperand()
        {
            //return 10;
            return Exponential.GetNext(0, 0, 2);
        }

        public static double GetAdvanceOperand()
        {
            //return 60;
            return Exponential.GetNext(0, 0, 10);
        }
    }
}
