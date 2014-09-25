using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler;
using MyAss.Compiler_v2.CodeGeneration;
using NUnit.Framework;

namespace MyAss.Compiler_v2.Tests.Run
{
    [TestFixture]
    [Category("Run_v2_Model")]
    public class Run_Model
    {
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

        [Test]
        public void Model_TableStdDevTest()
        {
            string input = TestModels.TableStdSevTest;
            CommonCode(input);
        }
        
        private static void CommonCode(string input)
        {
            AssemblyCompiler compiler = new AssemblyCompiler(input, true);
            compiler.Compile(true);
            compiler.RunAssembly();
        }
    }
}
