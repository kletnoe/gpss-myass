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
    [Category("Build_Model")]
    public class BuildTests_Model
    {
        [Test]
        public void Model_Dynamic()
        {
            string input = TestModels.MM3Model_Dynamic;
            CommonCode(input);
        }

        [Test]
        public void Model_TurnstaleDemo()
        {
            string input = TestModels.Model_TurnstaleDemo;
            CommonCode(input);
        }

        [Test]
        public void Model()
        {
            string input = TestModels.MM3Model_Simple;
            CommonCode(input);
        }

        [Test]
        public void Model_WithTable()
        {
            string input = TestModels.MM3Model_WithTable;
            CommonCode(input);
        }
        
        private static void CommonCode(string input)
        {
            AssemblyCompiler compiler = new AssemblyCompiler(input, true);
            compiler.Compile(true);

            Console.WriteLine(GenerationUtils.PrintCodeObject(compiler.CompileUnit));
        }
    }
}
