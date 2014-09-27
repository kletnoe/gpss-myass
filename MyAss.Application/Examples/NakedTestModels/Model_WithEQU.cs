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

nnnnn
";
            }
        }
    }
}
