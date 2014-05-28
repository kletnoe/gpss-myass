using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.CodeGeneration
{
    public class CodeGenerationsExample
    {
        CodeNamespace mynamespace;
        CodeTypeDeclaration myclass;
        CodeCompileUnit myassembly;

        public void CreateNamespace()
        {
            mynamespace = new CodeNamespace("mynamespace");
        }

        public void CreateImports()
        {
            mynamespace.Imports.Add(new CodeNamespaceImport("System"));
            mynamespace.Imports.Add(new CodeNamespaceImport("System.Drawing"));
            mynamespace.Imports.Add(new CodeNamespaceImport("System.Windows.Forms"));
        }

        public void CreateClass()
        {
            myclass = new CodeTypeDeclaration();
            //Assign a name for the class
            myclass.Name = "CMyclass";
            myclass.IsClass = true;
            //Set the Access modifier.
            myclass.Attributes = MemberAttributes.Public;
            //Add the newly created class to the namespace
            mynamespace.Types.Add(myclass);
        }

        public void CreateMember()
        {
            //Provide the type and variable name 
            CodeMemberField mymemberfield = new CodeMemberField(typeof(System.String), "strMessage");
            //Add the member to the class
            myclass.Members.Add(mymemberfield);
        }

        public void CreateProperty()
        {
            CodeMemberProperty mymemberproperty = new CodeMemberProperty();
            //Name of the property
            mymemberproperty.Name = "Message";
            //Data type of the property
            mymemberproperty.Type = new CodeTypeReference(typeof(System.String));
            //Access modifier of the property
            mymemberproperty.Attributes = MemberAttributes.Public;
            //Add the property to the class
            myclass.Members.Add(mymemberproperty);
            //Add the code-snippets to the property. 
            //If required, we can also add some custom validation code.
            //using the CodeSnippetExpression class.
            //Provide the return <propertyvalue> statement-For getter
            CodeSnippetExpression getsnippet = new CodeSnippetExpression("return strMessage");
            //Assign the new value to the property-For setter
            CodeSnippetExpression setsnippet = new CodeSnippetExpression("strMessage=value");
            //Add the code snippets into the property
            mymemberproperty.GetStatements.Add(getsnippet);
            mymemberproperty.SetStatements.Add(setsnippet);
        }

        public void CreateMethod()
        {
            //Create an object of the CodeMemberMethod
            CodeMemberMethod mymethod = new CodeMemberMethod();
            //Assign a name for the method.
            mymethod.Name = "AddNumbers";
            //create two parameters
            CodeParameterDeclarationExpression cpd1 = new CodeParameterDeclarationExpression(typeof(int), "a");
            CodeParameterDeclarationExpression cpd2 = new CodeParameterDeclarationExpression(typeof(int), "b");
            //Add the parameters to the method.
            mymethod.Parameters.AddRange(new CodeParameterDeclarationExpression[] { cpd1, cpd2 });
            //Provide the return type for the method.
            CodeTypeReference ctr = new CodeTypeReference(typeof(System.Int32));
            //Assign the return type to the method.
            mymethod.ReturnType = ctr;
            //Provide definition to the method (returns the sum of two //numbers)
            CodeSnippetExpression snippet1 = new CodeSnippetExpression("System.Console.WriteLine(\" Adding :\" + a + \" And \" + b )");
            //return the value
            CodeSnippetExpression snippet2 = new CodeSnippetExpression("return a+b");
            //Convert the snippets into Expression statements.
            CodeExpressionStatement stmt1 = new CodeExpressionStatement(snippet1);
            CodeExpressionStatement stmt2 = new CodeExpressionStatement(snippet2);
            //Add the expression statements to the method.
            mymethod.Statements.Add(stmt1);
            mymethod.Statements.Add(stmt2);
            //Provide the access modifier for the method.
            mymethod.Attributes = MemberAttributes.Public;
            //Finally add the method to the class.
            myclass.Members.Add(mymethod);
        }

        public void CreateEntryPoint()
        {
            //Create an object and assign the name as "Main"
            CodeEntryPointMethod mymain = new CodeEntryPointMethod();
            mymain.Name = "Main";
            //Mark the access modifier for the main method as Public and //static
            mymain.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            //Provide defenition to the main method. 
            //Create an object of the "Cmyclass" and invoke the method
            //by passing the required parameters.
            CodeSnippetExpression exp1 = new CodeSnippetExpression("CMyclass x = new CMyclass()");
            //Assign value to our property
            CodeSnippetExpression exp2 = new CodeSnippetExpression("x.Message=\"Hello World \"");
            //Print the value in the property
            CodeSnippetExpression exp3 = new CodeSnippetExpression("Console.WriteLine(x.Message)");
            //Invode the method
            CodeSnippetExpression exp4 = new CodeSnippetExpression("Console.WriteLine(\"Answer: {0}\",x.AddNumbers(10,20))");
            CodeSnippetExpression exp5 = new CodeSnippetExpression("int x = 2 + 4 + 5");
            //Create expression statements for the snippets
            CodeExpressionStatement ces1 = new CodeExpressionStatement(exp1);
            CodeExpressionStatement ces2 = new CodeExpressionStatement(exp2);
            CodeExpressionStatement ces3 = new CodeExpressionStatement(exp3);
            CodeExpressionStatement ces4 = new CodeExpressionStatement(exp4);
            CodeExpressionStatement ces5 = new CodeExpressionStatement(exp5);
            //Add the expression statements to the main method. 
            mymain.Statements.Add(ces1);
            mymain.Statements.Add(ces2);
            mymain.Statements.Add(ces3);
            mymain.Statements.Add(ces4);
            mymain.Statements.Add(ces5);
            //Add the main method to the class
            myclass.Members.Add(mymain);
        }

        public void SaveAssembly()
        {
            //Create a new object of the global CodeCompileUnit class.
            myassembly = new CodeCompileUnit();
            //Add the namespace to the assembly.
            myassembly.Namespaces.Add(mynamespace);
            //Add the following compiler parameters. (The references to the //standard .net dll(s) and framework library).
            CompilerParameters comparam = new CompilerParameters(new string[] { "mscorlib.dll" });
            comparam.ReferencedAssemblies.Add("System.dll");
            comparam.ReferencedAssemblies.Add("System.Drawing.dll"); comparam.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            //Indicates Whether the compiler has to generate the output in //memory
            comparam.GenerateInMemory = false;
            //Indicates whether the output is an executable.
            comparam.GenerateExecutable = true;
            //provide the name of the class which contains the Main Entry //point method
            comparam.MainClass = "mynamespace.CMyclass";
            //provide the path where the generated assembly would be placed 
            comparam.OutputAssembly = @"c:\temp\HelloWorld.exe";
            //Create an instance of the c# compiler and pass the assembly to //compile
            Microsoft.CSharp.CSharpCodeProvider ccp = new Microsoft.CSharp.CSharpCodeProvider();
            ICodeCompiler icc = ccp.CreateCompiler();
            //The CompileAssemblyFromDom would either return the list of 
            //compile time errors (if any), or would create the 
            //assembly in the respective path in case of successful //compilation
            CompilerResults compres = icc.CompileAssemblyFromDom(comparam, myassembly);
            if (compres == null || compres.Errors.Count > 0)
            {
                for (int i = 0; i < compres.Errors.Count; i++)
                {
                    Console.WriteLine(compres.Errors[i]);
                }
            }
        }
    }
}
