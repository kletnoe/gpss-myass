using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModels
{
    public class MM3Model_WithTable : MyAss.Framework_v2.AbstractModel
    {
        private int Server;

        private int OnQueue;

        private int OnQueueTime;

        private int GenerateCounter;

        private int Tail;

        private int GoAway;

        private int RejetionProb;

        private int RejectCounter;

        public MM3Model_WithTable()
        {
            // 
            // Server STORAGE 3
            this.Server = 10000;
            this.AddName(this.Server, "Server");
            if (true)
            {
                MyAss.Framework_v2.BuiltIn.Commands.Storage verb = new MyAss.Framework_v2.BuiltIn.Commands.Storage(new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb1_Operand1)));
                verb.SetId(this.Server);
                this.AddVerb(verb);
            }
            // 
            // OnQueue TABLE MP$OnQueueTime,0,4,20
            this.OnQueue = 10001;
            this.AddName(this.OnQueue, "OnQueue");
            this.OnQueueTime = 10002;
            this.AddName(this.OnQueueTime, "OnQueueTime");
            if (true)
            {
                MyAss.Framework_v2.TablePackage.Commands.Table verb = new MyAss.Framework_v2.TablePackage.Commands.Table(new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb2_Operand1)), new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb2_Operand2)), new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb2_Operand3)), new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb2_Operand4)));
                verb.SetId(this.OnQueue);
                this.AddVerb(verb);
            }
            // 
            // START 10000
            if (true)
            {
                MyAss.Framework_v2.BuiltIn.Commands.Start verb = new MyAss.Framework_v2.BuiltIn.Commands.Start(new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb3_Operand1)), null, null, null);
                this.AddVerb(verb);
            }
            // 
            // GENERATE Exponential(1, 0, (1 / 2))
            if (true)
            {
                MyAss.Framework_v2.BuiltIn.Blocks.Generate verb = new MyAss.Framework_v2.BuiltIn.Blocks.Generate(new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb4_Operand1)), null, null, null, null);
                this.AddVerb(verb);
            }
            // 
            // MARK OnQueueTime
            if (true)
            {
                MyAss.Framework_v2.BuiltIn.Blocks.Mark verb = new MyAss.Framework_v2.BuiltIn.Blocks.Mark(new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb5_Operand1)));
                this.AddVerb(verb);
            }
            // 
            // SAVEVALUE GenerateCounter,X$GenerateCounter
            this.GenerateCounter = 10003;
            this.AddName(this.GenerateCounter, "GenerateCounter");
            if (true)
            {
                MyAss.Framework_v2.BuiltIn.Blocks.Savevalue verb = new MyAss.Framework_v2.BuiltIn.Blocks.Savevalue(new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb6_Operand1)), new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb6_Operand2)));
                this.AddVerb(verb);
            }
            // 
            // TEST L,Q$Tail,20,GoAway,,
            this.Tail = 10004;
            this.AddName(this.Tail, "Tail");
            this.GoAway = 10005;
            this.AddName(this.GoAway, "GoAway");
            if (true)
            {
                MyAss.Framework_v2.BuiltIn.Blocks.Test verb = new MyAss.Framework_v2.BuiltIn.Blocks.Test(new MyAss.Framework_v2.OperandTypes.LiteralOperand("L"), new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb7_Operand2)), new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb7_Operand3)), new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb7_Operand4)));
                this.AddVerb(verb);
            }
            // 
            // QUEUE Tail
            if (true)
            {
                MyAss.Framework_v2.BuiltIn.Blocks.Queue verb = new MyAss.Framework_v2.BuiltIn.Blocks.Queue(new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb8_Operand1)), null);
                this.AddVerb(verb);
            }
            // 
            // ENTER Server,1
            if (true)
            {
                MyAss.Framework_v2.BuiltIn.Blocks.Enter verb = new MyAss.Framework_v2.BuiltIn.Blocks.Enter(new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb9_Operand1)), new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb9_Operand2)));
                this.AddVerb(verb);
            }
            // 
            // DEPART Tail
            if (true)
            {
                MyAss.Framework_v2.BuiltIn.Blocks.Depart verb = new MyAss.Framework_v2.BuiltIn.Blocks.Depart(new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb10_Operand1)), null);
                this.AddVerb(verb);
            }
            // 
            // TABULATE OnQueue
            if (true)
            {
                MyAss.Framework_v2.TablePackage.Blocks.Tabulate verb = new MyAss.Framework_v2.TablePackage.Blocks.Tabulate(new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb11_Operand1)), null);
                this.AddVerb(verb);
            }
            // 
            // ADVANCE Exponential(2, 0, (1 / 0.2))
            if (true)
            {
                MyAss.Framework_v2.BuiltIn.Blocks.Advance verb = new MyAss.Framework_v2.BuiltIn.Blocks.Advance(new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb12_Operand1)), null);
                this.AddVerb(verb);
            }
            // 
            // LEAVE Server
            if (true)
            {
                MyAss.Framework_v2.BuiltIn.Blocks.Leave verb = new MyAss.Framework_v2.BuiltIn.Blocks.Leave(new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb13_Operand1)), new MyAss.Framework_v2.OperandTypes.ParExpression(() => 1));
                this.AddVerb(verb);
            }
            // 
            // SAVEVALUE RejetionProb,(X$RejectCounter / X$GenerateCounter)
            this.RejetionProb = 10006;
            this.AddName(this.RejetionProb, "RejetionProb");
            this.RejectCounter = 10007;
            this.AddName(this.RejectCounter, "RejectCounter");
            if (true)
            {
                MyAss.Framework_v2.BuiltIn.Blocks.Savevalue verb = new MyAss.Framework_v2.BuiltIn.Blocks.Savevalue(new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb14_Operand1)), new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb14_Operand2)));
                this.AddVerb(verb);
            }
            // 
            // TERMINATE 1
            if (true)
            {
                MyAss.Framework_v2.BuiltIn.Blocks.Terminate verb = new MyAss.Framework_v2.BuiltIn.Blocks.Terminate(new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb15_Operand1)));
                this.AddVerb(verb);
            }
            // 
            // GoAway SAVEVALUE RejectCounter,X$RejectCounter
            this.GoAway = 13;
            this.ReplaceNameId(this.GoAway, "GoAway");
            if (true)
            {
                MyAss.Framework_v2.BuiltIn.Blocks.Savevalue verb = new MyAss.Framework_v2.BuiltIn.Blocks.Savevalue(new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb16_Operand1)), new MyAss.Framework_v2.OperandTypes.ParExpression(new MyAss.Framework_v2.OperandTypes.ExpressionDelegate(this.Verb16_Operand2)));
                this.AddVerb(verb);
            }
            // 
            // TERMINATE 
            if (true)
            {
                MyAss.Framework_v2.BuiltIn.Blocks.Terminate verb = new MyAss.Framework_v2.BuiltIn.Blocks.Terminate(null);
                this.AddVerb(verb);
            }
        }

        public virtual double Verb1_Operand1()
        {
            double result;
            result = 3D;
            return result;
        }

        public virtual double Verb2_Operand1()
        {
            double result;
            result = MyAss.Framework_v2.SNA.SystemSna.MP(this.simulation, this.OnQueueTime);
            return result;
        }

        public virtual double Verb2_Operand2()
        {
            double result;
            result = 0D;
            return result;
        }

        public virtual double Verb2_Operand3()
        {
            double result;
            result = 4D;
            return result;
        }

        public virtual double Verb2_Operand4()
        {
            double result;
            result = 20D;
            return result;
        }

        public virtual double Verb3_Operand1()
        {
            double result;
            result = 10000D;
            return result;
        }

        public virtual double Verb4_Operand1()
        {
            double result;
            result = MyAss.Framework.Procedures.Distributions.Exponential(1D, 0D, (1D / 2D));
            return result;
        }

        public virtual double Verb5_Operand1()
        {
            double result;
            result = this.OnQueueTime;
            return result;
        }

        public virtual double Verb6_Operand1()
        {
            double result;
            result = this.GenerateCounter;
            return result;
        }

        public virtual double Verb6_Operand2()
        {
            double result;
            result = MyAss.Framework_v2.BuiltIn.SNA.SavevalueSNA.X(this.simulation, this.GenerateCounter);
            return result;
        }

        public virtual double Verb7_Operand2()
        {
            double result;
            result = MyAss.Framework_v2.BuiltIn.SNA.QueueSNA.Q(this.simulation, this.Tail);
            return result;
        }

        public virtual double Verb7_Operand3()
        {
            double result;
            result = 20D;
            return result;
        }

        public virtual double Verb7_Operand4()
        {
            double result;
            result = this.GoAway;
            return result;
        }

        public virtual double Verb8_Operand1()
        {
            double result;
            result = this.Tail;
            return result;
        }

        public virtual double Verb9_Operand1()
        {
            double result;
            result = this.Server;
            return result;
        }

        public virtual double Verb9_Operand2()
        {
            double result;
            result = 1D;
            return result;
        }

        public virtual double Verb10_Operand1()
        {
            double result;
            result = this.Tail;
            return result;
        }

        public virtual double Verb11_Operand1()
        {
            double result;
            result = this.OnQueue;
            return result;
        }

        public virtual double Verb12_Operand1()
        {
            double result;
            result = MyAss.Framework.Procedures.Distributions.Exponential(2D, 0D, (1D / 0.2D));
            return result;
        }

        public virtual double Verb13_Operand1()
        {
            double result;
            result = this.Server;
            return result;
        }

        public virtual double Verb14_Operand1()
        {
            double result;
            result = this.RejetionProb;
            return result;
        }

        public virtual double Verb14_Operand2()
        {
            double result;
            result = (MyAss.Framework_v2.BuiltIn.SNA.SavevalueSNA.X(this.simulation, this.RejectCounter) / MyAss.Framework_v2.BuiltIn.SNA.SavevalueSNA.X(this.simulation, this.GenerateCounter));
            return result;
        }

        public virtual double Verb15_Operand1()
        {
            double result;
            result = 1D;
            return result;
        }

        public virtual double Verb16_Operand1()
        {
            double result;
            result = this.RejectCounter;
            return result;
        }

        public virtual double Verb16_Operand2()
        {
            double result;
            result = MyAss.Framework_v2.BuiltIn.SNA.SavevalueSNA.X(this.simulation, this.RejectCounter);
            return result;
        }
    }
}
