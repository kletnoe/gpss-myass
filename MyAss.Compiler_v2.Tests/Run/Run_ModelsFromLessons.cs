using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler;
using NUnit.Framework;

namespace MyAss.Compiler_v2.Tests.Run
{
    [TestFixture]
    [Category("Run_v2_ModelsFromLessons")]
    public class Run_ModelsFromLessons
    {
        [Test]
        public void L1_WithStorage()
        {
            string input = @"
In_use EQU 5 ;Mean time
Range EQU 3 ;Half range
Server STORAGE 1

START 3000
        GENERATE  7,7           ;People arrive
        QUEUE     Turn          ;Enter queue
        ENTER     Server        ;Acquire turnstile
        DEPART    Turn          ;Depart the queue
        ADVANCE   In_use,Range  ;Use turnstile
        LEAVE   Server          ;Leave turnstile
        TERMINATE 1             ;One spectator enters
";
            CommonCode(input);
        }

        private static void CommonCode(string input)
        {
            input = Defaults.DefUsing + input;

            AssemblyCompiler compiler = new AssemblyCompiler(input, true);
            compiler.Compile(true);
            compiler.RunAssembly();
        }
    }
}