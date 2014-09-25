using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler_v2.Tests
{
    public static class TestModels
    {
        public static String MM3Model_Simple
        {
            get
            {
                return @"
@using MyAss.Framework_v2.BuiltIn.Blocks
@using MyAss.Framework_v2.BuiltIn.Commands

@usingp MyAss.Framework_v2.BuiltIn.SNA.SavevalueSNA
@usingp MyAss.Framework_v2.BuiltIn.SNA.QueueSNA
@usingp MyAss.Framework.Procedures.Distributions

Server STORAGE 3

	START 10000

	GENERATE (Exponential(1,0,1/2))
	SAVEVALUE GenerateCounter,X$GenerateCounter+1

	TEST L Q$Tail,20,GoAway		;Jump if in Stack >20
	QUEUE Tail
	ENTER Server,1
	DEPART Tail
	ADVANCE (Exponential(2,0,1/0.2))
	LEAVE Server,1

	SAVEVALUE RejetionProb,(X$RejectCounter/X$GenerateCounter)
	TERMINATE 1


GoAway	SAVEVALUE RejectCounter,X$RejectCounter+1
	TERMINATE 		;Delete rejected.

";
            }
        }

        public static String MM3Model_Dynamic
        {
            get
            {
                return @"
@using MyAss.Framework_v2.BuiltIn.Blocks
@using MyAss.Framework_v2.BuiltIn.Commands

@usingp MyAss.Framework.Procedures.Distributions

Server STORAGE 3

	START 10000

	GENERATE (Exponential(1,0,1/2))
	SAVEVALUE GenerateCounter,X$GenerateCounter+1

	TEST L Q$Tail,20,GoAway		;Jump if in Stack >20
	QUEUE Tail
	ENTER Server,1
	DEPART Tail
	ADVANCE (Exponential(2,0,1/0.2))
	LEAVE Server,1

	SAVEVALUE RejetionProb,(X$RejectCounter/X$GenerateCounter)
	TERMINATE 1


GoAway	SAVEVALUE RejectCounter,X$RejectCounter+1
	TERMINATE 		;Delete rejected.

";
            }
        }

        public static String MM3Model_WithTable
        {
            get
            {
                return @"
@using MyAss.Framework_v2.BuiltIn.Blocks
@using MyAss.Framework_v2.BuiltIn.Commands
@using MyAss.Framework_v2.TablePackage.Blocks
@using MyAss.Framework_v2.TablePackage.Commands

@usingp MyAss.Framework_v2.SNA.SystemSna
@usingp MyAss.Framework_v2.BuiltIn.SNA.SavevalueSNA
@usingp MyAss.Framework_v2.BuiltIn.SNA.QueueSNA
@usingp MyAss.Framework.Procedures.Distributions

Server STORAGE 3

;InSystem TABLE MP$InSystemTime,0,4,20
;OnServer TABLE MP$OnServTime,0,2,20
OnQueue TABLE MP$OnQueueTime,0,4,20


	START 10000

	GENERATE (Exponential(1,0,1/2))
;		MARK InSystemTime
		MARK OnQueueTime
	SAVEVALUE GenerateCounter,(X$GenerateCounter+1)

	TEST L Q$Tail,20,GoAway		;Jump if in Stack >20
	QUEUE Tail
	ENTER Server,1
	DEPART Tail
		TABULATE OnQueue
;		MARK OnServTime
	ADVANCE (Exponential(2,0,1/0.2))
	LEAVE Server,1
;		TABULATE OnServer		
;		TABULATE InSystem

	SAVEVALUE RejetionProb,(X$RejectCounter/X$GenerateCounter)
	TERMINATE 1


GoAway	SAVEVALUE RejectCounter,(X$RejectCounter+1)
	TERMINATE 		;Delete rejected.

";
            }
        }


        public static String TableStdSevTest
        {
            get
            {
                return @"
@using MyAss.Framework_v2.BuiltIn.Blocks
@using MyAss.Framework_v2.BuiltIn.Commands
@using MyAss.Framework_v2.TablePackage.Blocks
@using MyAss.Framework_v2.TablePackage.Commands

@usingp MyAss.Framework_v2.SNA.SystemSna
@usingp MyAss.Framework_v2.BuiltIn.SNA.SavevalueSNA
@usingp MyAss.Framework_v2.BuiltIn.SNA.QueueSNA
@usingp MyAss.Framework.Procedures.Distributions

TheTable TABLE P$const,0,1,20
START 1

	GENERATE ,,1,1
	ASSIGN const,3
	TABULATE TheTable
	TERMINATE 

	GENERATE ,,2,1
	ASSIGN const,8
	TABULATE TheTable
	TERMINATE

	GENERATE ,,3,1
	ASSIGN const,1
	TABULATE TheTable
	TERMINATE

	GENERATE ,,4,1
	ASSIGN const,5
	TABULATE TheTable
	TERMINATE

	GENERATE ,,5,1
	ASSIGN const,14
	TABULATE TheTable
	TERMINATE

	GENERATE ,,6,1
	ASSIGN const,6
	TABULATE TheTable
	TERMINATE 1
";
            }
        }
    }
}
