using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler.Tests
{
    public static class Defaults
    {
        public const string DefUsing = @"
@using MyAss.Framework.BuiltIn.Blocks
@using MyAss.Framework.BuiltIn.Commands
@using MyAss.Framework.TablePackage.Blocks
@using MyAss.Framework.TablePackage.Commands

@usingp MyAss.Framework.BuiltIn.SNA.SystemSNA
@usingp MyAss.Framework.BuiltIn.SNA.SavevalueSNA
@usingp MyAss.Framework.BuiltIn.SNA.QueueSNA
@usingp MyAss.Framework.Procedures.Distributions
";
    }
}
