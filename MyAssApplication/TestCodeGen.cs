using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler.CodeGeneration;

namespace MyAssApplication
{
    public static class TestCodeGen
    {
        public static void Run()
        {
            CodeGenerationsExample cg = new CodeGenerationsExample();
            cg.CreateNamespace();
            cg.CreateImports();
            cg.CreateClass();
            cg.CreateMember();
            cg.CreateProperty();
            cg.CreateMethod();
            cg.CreateEntryPoint();
            cg.SaveAssembly();
            Console.ReadLine();
        }
    }
}
