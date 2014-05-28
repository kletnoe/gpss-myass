using System;
using System.Collections.Generic;
//using System.Linq;
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
            //List<String> pizda = new List<String>();
            //pizda.Add("a");
            //pizda.Add("b");
            //pizda.Add("c");
            //pizda.Add("d");
            //pizda.Add("e");

            //List<String> subPizda = pizda.GetRange(2, 2);
            //subPizda.Clear();

            //foreach (var item in pizda)
            //{
            //    Console.WriteLine(item);
            //}

            //foreach (var item in new MetadataRetriever().GetAllDefinitions())
            //{
            //    Console.WriteLine(item); 
            //}


            //ASTModel model = new SimpleASTExample().GetAst();
            //Console.WriteLine(model.ToString());


            TestCodeGen.Run();
            Console.WriteLine();
            //Model.RunDefaultModel();
            //Compiler.TestCodeGen();
        }
    }
}
