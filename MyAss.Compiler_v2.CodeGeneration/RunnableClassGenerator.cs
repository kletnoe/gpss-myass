using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2;
using MyAss.Utilities.Reports_v2;

namespace MyAss.Compiler_v2.CodeGeneration
{
    public static class RunnableClassGenerator
    {
        private const string RunnableClassName = "Program";
        private const string SimulationVarName = "simulation";
        private const string PrintReportMethodName = "PrintReport";

        public static CodeTypeDeclaration ConstructRunnableClass(string modelClassName)
        {
            CodeTypeDeclaration theClass = new CodeTypeDeclaration();
            theClass.Attributes = MemberAttributes.Public;
            theClass.Name = RunnableClassName;

            theClass.Members.Add(ConstructMainMethod(modelClassName));

            return theClass;
        }

        /*
        public static void Main()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Restart();

            var sim = new MyAss.Framework_v2.Simulation(new MM3Model());

            sw.Stop();
            System.Console.WriteLine("Time elapsed: " + sw.Elapsed);

            MyAss.Utilities.Reports_v2.StandardReport.PrintReport(sim);

            System.Console.WriteLine();
        }
        */
        private static CodeEntryPointMethod ConstructMainMethod(string modelClassName)
        {
            CodeEntryPointMethod mainMethod = new CodeEntryPointMethod();

            mainMethod.Statements.Add(new CodeSnippetStatement(@"var sw = new System.Diagnostics.Stopwatch();"));
            mainMethod.Statements.Add(new CodeSnippetStatement(@"sw.Restart();"));

            mainMethod.Statements.Add(ConstrucSimulationDeclaration(modelClassName));

            mainMethod.Statements.Add(new CodeSnippetStatement(@"sw.Stop();"));
            mainMethod.Statements.Add(new CodeSnippetStatement(@"System.Console.WriteLine(""Time elapsed: "" + sw.Elapsed);"));

            mainMethod.Statements.Add(ConstructPrintReportMethodCall());

            mainMethod.Statements.Add(new CodeSnippetStatement(@"System.Console.WriteLine();"));

            return mainMethod;
        }

        private static CodeStatement ConstrucSimulationDeclaration(string modelClassName)
        {
            CodeVariableDeclarationStatement decl = new CodeVariableDeclarationStatement();
            decl.Type = new CodeTypeReference(typeof(Simulation));
            decl.Name = SimulationVarName;
            decl.InitExpression = new CodeObjectCreateExpression(
                typeof(Simulation),
                new CodeObjectCreateExpression(
                    modelClassName
                )
            );

            return decl;
        }

        private static CodeExpressionStatement ConstructPrintReportMethodCall()
        {
            CodeMethodInvokeExpression methodCall = new CodeMethodInvokeExpression();
            methodCall.Method = new CodeMethodReferenceExpression();
            methodCall.Method.TargetObject = new CodeTypeReferenceExpression(typeof(StandardReport));
            methodCall.Method.MethodName = PrintReportMethodName;

            methodCall.Parameters.Add(new CodeVariableReferenceExpression(SimulationVarName));

            return new CodeExpressionStatement(methodCall);
        }
    }
}
