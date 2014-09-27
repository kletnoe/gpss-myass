using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Application.Examples.NakedTestModels
{
    public static class Model_WithEQU
    {
        public static string Model
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

In_use EQU 5 ;Mean time
Range EQU 3 ;Half range
Server STORAGE 1

START 30000
        GENERATE  7,7           ;People arrive
        QUEUE     Turn          ;Enter queue
        ENTER     Server          ;Acquire turnstile
        DEPART    Turn          ;Depart the queue
        ADVANCE   In_use,Range  ;Use turnstile
        LEAVE   Server          ;Leave turnstile
        TERMINATE 1             ;One spectator enters
";
            }
        }
    }
}
