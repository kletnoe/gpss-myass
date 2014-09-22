﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler;
using MyAss.Compiler_v2.CodeGeneration;
using NUnit.Framework;

namespace MyAss.Compiler_v2.Tests.Build
{
    [TestFixture]
    [Category("Build_v2_Model")]
    public class BuildTests_Model
    {
        [Test]
        public void Model()
        {
            string input = @"
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
            CommonCode(input);
        }

        private static void CommonCode(string input)
        {
            AssemblyCompiler compiler = new AssemblyCompiler(input, true);
            compiler.Compile(true);

            Assert.Pass(GenerationUtils.PrintCodeObject(compiler.CompileUnit));
        }
    }
}