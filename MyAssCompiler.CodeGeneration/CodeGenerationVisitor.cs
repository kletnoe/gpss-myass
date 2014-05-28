using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler.AST;

namespace MyAssCompiler.CodeGeneration
{
    public class CodeGenerationVisitor : IASTVisitor
    {
        private IParser parser;

        const string namespaceName = "Modeling";
        const string outputPath = @"c:\temp\HelloWorld.dll";

        private int currentModelNo = 1;
        private int currentBlockNo = 1;
        private int currentOperandNo = 1;

        private string CurrentMethodName
        {
            get
            {
                return "Block" + this.currentBlockNo + "_Operand" + this.currentOperandNo;
            }
        }

        private string CurrentClassName
        {
            get
            {
                return "Model" + this.currentModelNo;
            }
        }


        CodeNamespace rootnamespace;
        CodeTypeDeclaration currentClass;
        CodeMemberMethod currentMethod;
        //statement not used?
        CodeExpression currentExpression;

        public CodeGenerationVisitor(IParser parser)
        {
            this.parser = parser;
        }

        public void Run()
        {
            var model = parser.Parse();

            rootnamespace = new CodeNamespace(namespaceName);
            CodeCompileUnit theAssembly = new CodeCompileUnit();
            theAssembly.Namespaces.Add(rootnamespace);

            //Add the following compiler parameters. (The references to the //standard .net dll(s) and framework library).
            CompilerParameters compilerParams = new CompilerParameters(new string[] { "mscorlib.dll" });
            compilerParams.ReferencedAssemblies.Add("System.dll");

            //Indicates Whether the compiler has to generate the output in //memory
            compilerParams.GenerateInMemory = false;
            //Indicates whether the output is an executable.
            compilerParams.GenerateExecutable = false;

            //provide the name of the class which contains the Main Entry //point method
            //compilerParams.MainClass = mainClassName;
            //provide the path where the generated assembly would be placed 
            compilerParams.OutputAssembly = outputPath;

            model.Accept(this);

            //Create an instance of the c# compiler and pass the assembly to //compile
            Microsoft.CSharp.CSharpCodeProvider codeProvider = new Microsoft.CSharp.CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();

            //The CompileAssemblyFromDom would either return the list of 
            //compile time errors (if any), or would create the 
            //assembly in the respective path in case of successful //compilation
            CompilerResults compres = icc.CompileAssemblyFromDom(compilerParams, theAssembly);
            if (compres == null || compres.Errors.Count > 0)
            {
                for (int i = 0; i < compres.Errors.Count; i++)
                {
                    Console.WriteLine(compres.Errors[i]);
                }
            }
        }

        public void Visit(ASTModel node)
        {
            this.currentBlockNo = 1;

            currentClass = new CodeTypeDeclaration()
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Name = this.CurrentClassName
            };

            foreach (var verb in node.Verbs)
            {
                verb.Accept(this);
            }

            this.rootnamespace.Types.Add(this.currentClass);

            this.currentModelNo++;
        }

        public void Visit(ASTBlock node)
        {
            this.currentOperandNo = 1;
            node.Operands.Accept(this);
            this.currentBlockNo++;
        }

        public void Visit(ASTOperands node)
        {
            foreach (var operand in node.Operands)
            {
                if (operand != null)
                {
                    this.currentMethod = new CodeMemberMethod()
                    {
                        Attributes = MemberAttributes.Public | MemberAttributes.Static,
                        ReturnType = new CodeTypeReference(typeof(Double)),
                        Name = this.CurrentMethodName,
                    };

                    CodeVariableDeclarationStatement varDeclaration = new CodeVariableDeclarationStatement()
                    {
                        Type = new CodeTypeReference(typeof(Double)),
                        Name = "result"
                    };

                    CodeAssignStatement assignStatement = new CodeAssignStatement()
                    {
                        Left = new CodeVariableReferenceExpression("result")
                    };

                    ///////
                    operand.Accept(this);


                    assignStatement.Right = this.currentExpression;

                    CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement()
                    {
                        Expression = new CodeVariableReferenceExpression("result")
                    };

                    this.currentMethod.Statements.Add(varDeclaration);
                    this.currentMethod.Statements.Add(assignStatement);
                    this.currentMethod.Statements.Add(returnStatement);

                    this.currentClass.Members.Add(currentMethod);
                }

                this.currentOperandNo++;
            }
        }

        public void Visit(ASTExpression astExpr)
        {

            /* Main logic */


            if (astExpr.Operator == null)
            {
                astExpr.LTerm.Accept(this);
            }
            else
            {

            }

            /**/


        }

        public void Visit(ASTTerm astTerm)
        {
            if (astTerm.Operator == null)
            {
                astTerm.LFactor.Accept(this);
            }
            else
            {

            }
        }

        public void Visit(ASTSignedFactor astFactor)
        {
            if (astFactor.Operator == null)
            {
                astFactor.Value.Accept(this);
            }
            else
            {
                CodeBinaryOperatorExpression signedFactor = new CodeBinaryOperatorExpression();
                signedFactor.Left = new CodeSnippetExpression("1");

                switch (astFactor.Operator)
                {
                    case AddOperatorType.SUBSTRACT:
                        signedFactor.Operator = CodeBinaryOperatorType.Subtract;
                        break;
                    default:
                        signedFactor.Operator = CodeBinaryOperatorType.Add;
                        break;
                }

                astFactor.Value.Accept(this);

                signedFactor.Right = this.currentExpression;

                this.currentExpression = signedFactor;
            }
        }

        public void Visit(ASTCall node)
        {
            throw new NotImplementedException();
        }

        public void Visit(ASTSuffixOperator node)
        {
            throw new NotImplementedException();
        }

        public void Visit(ASTLValue node)
        {
            throw new NotImplementedException();
        }

        public void Visit(ASTLiteral astLiteral)
        {
            currentExpression = new CodeSnippetExpression(astLiteral.Value.ToString());
        }

        public void Visit(ASTDirectSNA node)
        {
            throw new NotImplementedException();
        }

        public void Visit(ASTOperator node)
        {
            throw new NotImplementedException();
        }

        public void Visit(ASTActuals node)
        {
            throw new NotImplementedException();
        }
    }
}
