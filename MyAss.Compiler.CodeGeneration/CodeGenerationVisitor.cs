using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MyAss.Compiler.AST;
using MyAss.Compiler.Metadata;
using MyAss.Framework;
using MyAss.Framework.Blocks;
using MyAss.Framework.Commands;

namespace MyAss.Compiler.CodeGeneration
{
    public class CodeGenerationVisitor : IASTVisitor
    {
        private IParser parser;

        private const string setLabelMethodName = "SetLabel";
        private const string namespaceName = "Modeling";
        private const string getModelMethodName = "GetModel";
        private const string resultModelVarName = "resultModel";
        private const string addVerbMethodName = "AddVerb";


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

        private CodeMemberMethod getModelMethod;
        private List<string> currentConstructorArgs;

        private CodeNamespace rootnamespace;
        private CodeTypeDeclaration currentClass;
        private CodeMemberMethod currentMethod;
        //statement not used?
        private CodeExpression currentExpression;

        public CodeGenerationVisitor(IParser parser)
        {
            this.parser = parser;
        }

        public void VisitAll()
        {
            this.rootnamespace = new CodeNamespace(namespaceName);
            var model = parser.Parse();
            model.Accept(this);
        }

        public CodeCompileUnit CreateAssembly()
        {
            CodeCompileUnit theAssembly = new CodeCompileUnit();

            //Referenced assemblies
            theAssembly.ReferencedAssemblies.Add("MyAss.Framework.dll");

            theAssembly.Namespaces.Add(rootnamespace);
            return theAssembly;
        }

        public void Run()
        {
            this.VisitAll();
            this.CreateAssembly();
        }

        public void Visit(ASTModel node)
        {
            this.currentBlockNo = 1;

            currentClass = new CodeTypeDeclaration()
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Name = this.CurrentClassName
            };

            /*
            Following scope constructs:
            public static Model GetModel()
            {
                Model resultModel = new Model();
                IBlock block;
                ICommand command;
            }
            */
            {
                CodeMemberMethod method = new CodeMemberMethod()
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Static,
                    ReturnType = new CodeTypeReference(typeof(Model)),
                    Name = getModelMethodName,
                };

                method.Statements.Add(
                    new CodeVariableDeclarationStatement(
                        typeof(Model),
                        resultModelVarName,
                        new CodeObjectCreateExpression(typeof(Model))
                    )
                );

                method.Statements.Add(
                    new CodeVariableDeclarationStatement(
                        typeof(IBlock),
                        "block"
                    )
                );

                method.Statements.Add(
                    new CodeVariableDeclarationStatement(
                        typeof(ICommand),
                        "command"
                    )
                );

                this.getModelMethod = method;
            }

            this.currentClass.Members.Add(this.getModelMethod);

            foreach (var verb in node.Verbs)
            {
                this.getModelMethod.Statements.Add(new CodeCommentStatement(" Verb declaration start."));
                verb.Accept(this);
                this.getModelMethod.Statements.Add(new CodeCommentStatement(" Verb declaration end."));
            }

            /*
            Following scope constructs:
                ...
                return resultModel;
                ...
            */
            {
                this.getModelMethod.Statements.Add(
                    new CodeMethodReturnStatement(
                        new CodeVariableReferenceExpression(
                            resultModelVarName
                        )
                    )
                );
            }

            this.rootnamespace.Types.Add(this.currentClass);

            this.currentModelNo++;
        }

        public void Visit(ASTVerb verb)
        {
            this.currentOperandNo = 1;
            verb.Operands.Accept(this);

            Type verbType = MetadataRetriever.GetBuiltinVerb(verb.VerbId);
            ConstructorInfo verbConstructor = verbType.GetConstructors().First();

            List<CodeExpression> constructorArgs = new List<CodeExpression>();

            for (int i = 0; i < verbConstructor.GetParameters().Length; i++)
            {
                if (this.currentConstructorArgs.Count > i)
                {
                    if (this.currentConstructorArgs[i] != null)
                    {
                        CodeObjectCreateExpression parExpr = new CodeObjectCreateExpression(
                            typeof(MyAss.Framework.OperandTypes.ParExpression),
                            new CodeDelegateCreateExpression(
                                new CodeTypeReference(
                                    typeof(MyAss.Framework.OperandTypes.ExpressionDelegate)),
                                new CodeTypeReferenceExpression(this.CurrentClassName),
                                "Block" + this.currentBlockNo + "_Operand" + (i+1)));

                        constructorArgs.Add(parExpr);
                    }
                    else
                    {
                        constructorArgs.Add(new CodePrimitiveExpression(null));
                    }
                }
                else
                {
                    constructorArgs.Add(new CodePrimitiveExpression(null));
                }
            }


            /*
            Following scope constructs:
                ...
                block = new BlockType(params);
                block.SetLabel(label);
                resultModel.Add(block);
                ...
            */
            if (typeof(IBlock).IsAssignableFrom(verbType))
            {
                this.getModelMethod.Statements.Add(
                    new CodeAssignStatement(
                        new CodeVariableReferenceExpression("block"),
                        new CodeObjectCreateExpression(verbType, constructorArgs.ToArray())
                    )
                );

                this.getModelMethod.Statements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeVariableReferenceExpression("block"),
                        setLabelMethodName,
                        new CodePrimitiveExpression(verb.LabelId)
                    )
                );

                this.getModelMethod.Statements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeVariableReferenceExpression(resultModelVarName),
                        addVerbMethodName,
                        new CodeVariableReferenceExpression("block")
                    )
                );
            }

            /*
            Following scope constructs:
                ...
                command = new CommandType(params);
                command.SetLabel(label);
                resultModel.Commands.Add(command);
                ...
            */
            if (typeof(ICommand).IsAssignableFrom(verbType))
            {
                this.getModelMethod.Statements.Add(
                    new CodeAssignStatement(
                        new CodeVariableReferenceExpression("command"),
                        new CodeObjectCreateExpression(verbType, constructorArgs.ToArray())
                    )
                );

                this.getModelMethod.Statements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeVariableReferenceExpression("command"),
                        setLabelMethodName,
                        new CodePrimitiveExpression(verb.LabelId)
                    )
                );

                this.getModelMethod.Statements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeVariableReferenceExpression(resultModelVarName),
                        addVerbMethodName,
                        new CodeVariableReferenceExpression("command")
                    )
                );
            }

            this.currentBlockNo++;
        }

        public void Visit(ASTOperands node)
        {
            this.currentConstructorArgs = new List<string>();

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

                    CodeVariableDeclarationStatement declareVarStatement
                        = new CodeVariableDeclarationStatement(typeof(Double), "result");

                    CodeAssignStatement assignStatement = new CodeAssignStatement();
                    assignStatement.Left = new CodeVariableReferenceExpression(declareVarStatement.Name);
                    
                    // Accept operand
                    operand.Accept(this);

                    assignStatement.Right = this.currentExpression;

                    CodeMethodReturnStatement returnStatement = 
                        new CodeMethodReturnStatement(new CodeVariableReferenceExpression(declareVarStatement.Name));

                    this.currentMethod.Statements.Add(declareVarStatement);
                    this.currentMethod.Statements.Add(assignStatement);
                    this.currentMethod.Statements.Add(returnStatement);

                    this.currentClass.Members.Add(this.currentMethod);

                    this.currentConstructorArgs.Add(this.currentMethod.Name);
                }
                else
                {
                    this.currentConstructorArgs.Add(null);
                }

                this.currentOperandNo++;
            }
        }

        public void Visit(ASTExpression astExpr)
        {
            if (astExpr.Additives.Count == 0)
            {
                astExpr.Term.Accept(this);
            }
            else
            {
                astExpr.Term.Accept(this);
                CodeExpression left = this.currentExpression;

                CodeBinaryOperatorExpression expr;
                foreach (var add in astExpr.Additives)
                {
                    expr = new CodeBinaryOperatorExpression();
                    expr.Left = left;
                    this.currentExpression = expr;
                    add.Accept(this);
                    left = this.currentExpression;
                }
            }
        }

        public void Visit(ASTAdditive additive)
        {
            CodeBinaryOperatorExpression expr = (CodeBinaryOperatorExpression)this.currentExpression;

            switch (additive.Operator)
            {
                case AddOperatorType.ADD:
                    expr.Operator = CodeBinaryOperatorType.Add;
                    break;
                case AddOperatorType.SUBSTRACT:
                    expr.Operator = CodeBinaryOperatorType.Subtract;
                    break;
            }

            additive.Term.Accept(this);
            expr.Right = this.currentExpression;

            this.currentExpression = expr;
        }

        public void Visit(ASTTerm term)
        {
            if (term.Multiplicatives.Count == 0)
            {
                term.Factor.Accept(this);
            }
            else
            {
                term.Factor.Accept(this);
                CodeExpression left = this.currentExpression;

                CodeBinaryOperatorExpression expr;
                foreach (var mult in term.Multiplicatives)
                {
                    expr = new CodeBinaryOperatorExpression();
                    expr.Left = left;
                    this.currentExpression = expr;
                    mult.Accept(this);
                    left = this.currentExpression;
                }
            }

        }

        public void Visit(ASTMultiplicative mult)
        {
            CodeBinaryOperatorExpression expr = (CodeBinaryOperatorExpression)this.currentExpression;

            switch (mult.Operator)
            {
                case MulOperatorType.MULTIPLY:
                    expr.Operator = CodeBinaryOperatorType.Multiply;
                    break;
                case MulOperatorType.DIVIDE:
                    expr.Operator = CodeBinaryOperatorType.Divide;
                    break;
                case MulOperatorType.MODULUS:
                    expr.Operator = CodeBinaryOperatorType.Modulus;
                    break;
                case MulOperatorType.EXPONENT:
                    throw new NotImplementedException("Exponentiation operator not implemented!");
            }

            mult.Factor.Accept(this);
            expr.Right = this.currentExpression;

            this.currentExpression = expr;
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
                signedFactor.Left = new CodePrimitiveExpression(0);

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

        public void Visit(ASTLValue lval)
        {
            if (lval.Accessor != null)
            {
                lval.Accessor.Accept(this);

                if (lval.Accessor is ASTDirectSNA)
                {
                    CodeMethodInvokeExpression expr = new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            new CodeTypeReferenceExpression(MetadataRetriever.GetBuiltinSnaType()),
                            lval.Id),
                        this.currentExpression);

                    this.currentExpression = expr;
                }
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
        public void Visit(ASTLiteral literal)
        {
            this.currentExpression = new CodePrimitiveExpression(literal.Value);
        }

        public void Visit(ASTDirectSNA sna)
        {
            this.currentExpression = new CodePrimitiveExpression(sna.Id);
        }

        public void Visit(ASTActuals node)
        {
            throw new NotImplementedException();
        }
    }
}
