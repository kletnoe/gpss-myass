using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler.AST;
using MyAssCompiler;
using MyAssCompiler.CodeGeneration;

namespace MyAssApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //foreach (var item in new MetadataRetriever().GetAllDefinitions())
            //{
            //    Console.WriteLine(item); 
            //}


            //ASTModel model = new SimpleASTExample().GetAst();
            //Console.WriteLine(model.ToString());


            TestParser.Run();
            Console.WriteLine();
            //Model.RunDefaultModel();
            //Compiler.TestCodeGen();
        }
    }
}
