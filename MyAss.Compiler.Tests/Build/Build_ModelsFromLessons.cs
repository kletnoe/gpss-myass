using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler;
using MyAss.Compiler.CodeGeneration;
using NUnit.Framework;

namespace MyAss.Compiler.Tests.Build
{
    [TestFixture]
    [Category("Build_ModelsFromLessons")]
    public class Build_ModelsFromLessons
    {
        [Test]
        public void L1_WithStorage()
        {
            string input = @"
In_use EQU 5 ;Mean time
Range EQU 3 ;Half range
Server STORAGE 1

START 300
        GENERATE  7,7           ;People arrive
        QUEUE     Turn          ;Enter queue
        ENTER     Server          ;Acquire turnstile
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

            Console.WriteLine(GenerationUtils.PrintCodeObject(compiler.CompileUnit));
        }
    }
}