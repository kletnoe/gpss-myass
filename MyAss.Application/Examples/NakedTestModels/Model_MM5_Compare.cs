using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Application.Examples.NakedTestModels
{
    public static class Model_MM5_Compare
    {
        public static string Model
        {
            get
            {
                return @"
@using MyAss.Framework.BuiltIn.Blocks
@using MyAss.Framework.BuiltIn.Commands
@using MyAss.Framework.TablePackage.Blocks
@using MyAss.Framework.TablePackage.Commands

@usingp MyAss.Framework.SNA.SystemSna
@usingp MyAss.Framework.BuiltIn.SNA.SavevalueSNA
@usingp MyAss.Framework.BuiltIn.SNA.QueueSNA
@usingp MyAss.Framework.Procedures.Distributions

Server STORAGE 5

InSystem TABLE MP$InSystemTime,0,0.4,25
OnQueue TABLE MP$OnQueueTime,0,0.4,15

    START 10000

    GENERATE (Exponential(1, 0, 1 / 10))
        MARK InSystemTime
        MARK OnQueueTime
    SAVEVALUE GenerateCounter,(X$GenerateCounter + 1)

    TEST L Q$Tail,20,GoReject; Jump if in Stack > 20
    QUEUE Tail
    ENTER Server,1
    DEPART Tail
        TABULATE OnQueue
    ADVANCE (Exponential(2, 0, 1 / 1.4))
    LEAVE Server,1
        TABULATE InSystem

    SAVEVALUE RejetionProb,(X$RejectCounter / X$GenerateCounter)
    TERMINATE 1

GoReject SAVEVALUE RejectCounter,(X$RejectCounter + 1)
	TERMINATE; Delete rejected.
";
            }
        }
    }
}
