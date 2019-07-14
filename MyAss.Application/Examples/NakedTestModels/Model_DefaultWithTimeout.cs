using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Application.Examples.NakedTestModels
{
    public static class Model_DefaultWithTimeout
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
@using MyCustomLibrary.Blocks

@usingp MyAss.Framework.BuiltIn.SNA.SystemSNA
@usingp MyAss.Framework.BuiltIn.SNA.SavevalueSNA
@usingp MyAss.Framework.BuiltIn.SNA.QueueSNA
@usingp MyAss.Framework.Procedures.Distributions


Server STORAGE 3

InSystem TABLE MP$InSystemTime,0,4,20
OnServer TABLE MP$OnServTime,0,2,20
OnQueue TABLE MP$OnQueueTime,0,4,20


	START 10000

	GENERATE (Exponential(5,0,1/2))
		MARK InSystemTime
		MARK OnQueueTime
	SAVEVALUE GenerateCounter,(X$GenerateCounter+1)

	TEST L Q$Tail,20,GoAway		;Jump if in Stack >20
	QUEUE Tail
    TIMEOUT 40,OnTimeout
	ENTER Server,1
	DEPART Tail
		TABULATE OnQueue
		MARK OnServTime
	ADVANCE (Exponential(2,0,1/0.2))
	LEAVE Server,1
		TABULATE OnServer		
		TABULATE InSystem

	SAVEVALUE RejetionProb,(X$RejectCounter/X$GenerateCounter)
    SAVEVALUE ExhaustProb,(X$TimeoutCounter/X$GenerateCounter)
	TERMINATE 1


GoAway	SAVEVALUE RejectCounter,(X$RejectCounter+1)
	TERMINATE 		;Delete rejected.

OnTimeout	SAVEVALUE TimeoutCounter,(X$TimeoutCounter+1)
    DEPART Tail
	TERMINATE 		;Delete exhausted.

";
            }
        }
    }
}
