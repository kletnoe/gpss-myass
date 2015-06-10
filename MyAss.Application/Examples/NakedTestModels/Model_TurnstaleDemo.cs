using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Application.Examples.NakedTestModels
{
    public static class Model_TurnstaleDemo
    {
        public static string Model
        {
            get
            {
                return @"
@using MyAss.Framework_v2.BuiltIn.Blocks
@using MyAss.Framework_v2.BuiltIn.Commands

Server STORAGE 1
START 10000

GENERATE 7,5
QUEUE Turn
ENTER Server
DEPART Turn
ADVANCE 5,3
LEAVE Server
TERMINATE 1
";
            }
        }
    }
}
