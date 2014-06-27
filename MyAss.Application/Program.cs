using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using MyAss.Compiler.AST;
using MyAss.Compiler;
using MyAss.Compiler.CodeGeneration;
using System.CodeDom;

namespace MyAss.Application
{
    class Program
    {
        class A
        {
            public int MyProperty { get; set; }
        }

        static void Main(string[] args)
        {
            A a = new A();


            List<A> aaa = new List<A>();
            aaa.Add(new A() { MyProperty = 1 });
            aaa.Add(new A() { MyProperty = 2 });
            aaa.Add(new A() { MyProperty = 3 });
            aaa.Add(new A() { MyProperty = 4 });
            aaa.Add(new A() { MyProperty = 1 });
            aaa.Add(new A() { MyProperty = 1 });


            //foreach (var item in new MetadataRetriever().GetAllDefinitions())
            //{
            //    Console.WriteLine(item); 
            //}


            //ASTModel model = new SimpleASTExample().GetAst();
            //Console.WriteLine(model.ToString());


            //TestCodeGen.Run();
            //Console.WriteLine();
            //Model.RunDefaultModel();
            //Compiler.TestCodeGen();
        }
    }
}

