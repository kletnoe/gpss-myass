using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler;
using MyAss.Compiler.Metadata;
using MyAss.Compiler_v2.AST;
using MyAss.Framework;
using MyAss.Framework.Blocks;
using MyAss.Framework.Commands;

namespace MyAss.Compiler_v2.CodeGeneration
{
    public class CodeDomGenerationVisitor : IASTVisitor<CodeObject>
    {
        private Parser_v2 parser;

        private const string theNamespaceName = "Modeling";
        private const string theClassName = "Model1";

        private const string setLabelMethodName = "SetLabel";
        private const string addVerbMethodName = "AddVerb";
        private const string getModelMethodName = "GetModel";
        private const string resultModelVarName = "resultModel";

        CodeNamespace theNamespace;
        CodeTypeDeclaration theClass;

        private int verbNo = 1;

        public CodeDomGenerationVisitor(Parser_v2 parser)
        {
            this.parser = parser;

            this.theClass = new CodeTypeDeclaration()
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                Name = CodeDomGenerationVisitor.theClassName
            };

            this.theNamespace = new CodeNamespace()
            {
                Name = CodeDomGenerationVisitor.theNamespaceName
            };

            theNamespace.Types.Add(theClass);
        }

        public CodeCompileUnit VisitAll()
        {
            ASTModel model = parser.Parse();

            model.Accept(this);

            CodeCompileUnit theAssembly = new CodeCompileUnit();
            theAssembly.ReferencedAssemblies.Add("MyAss.Framework.dll");
            theAssembly.ReferencedAssemblies.Add("MyAss.Framework.Procedures.dll");
            theAssembly.Namespaces.Add(theNamespace);

            return theAssembly;
        }

        public CodeObject Visit(ASTModel model)
        {
            /*
            Following scope constructs:
            public static Model GetModel()
            {
                Model resultModel = new Model();
            }
            */
            CodeMemberMethod method;
            {
                method = new CodeMemberMethod()
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
            }

            // Verbs
            foreach (var verb in model.Verbs)
            {
                method.Statements.Add((CodeStatement)verb.Accept(this));
                this.verbNo++;
            }

            /*
            Following scope constructs:
                ...
                return resultModel;
                ...
            */
            {
                method.Statements.Add(
                    new CodeMethodReturnStatement(
                        new CodeVariableReferenceExpression(
                            resultModelVarName
                        )
                    )
                );
            }

            this.theClass.Members.Add(method);

            return this.theClass;
        }

        public CodeObject Visit(ASTVerb verb)
        {
            Type verbType = MetadataRetriever.GetBuiltinVerb(verb.VerbId);
            int verbCtorParamsCount = verbType.GetConstructors().First().GetParameters().Length;

            CodeObjectCreateExpression ctorCall = new CodeObjectCreateExpression(verbType);

            for (int i = 0; i < verbCtorParamsCount; i++)
            {
                if (verb.Operands.Count > i && verb.Operands[i] != null)
                {
                    string operandMethodName = String.Format("Verb{0}_Operand{1}", this.verbNo, i + 1);
                    CodeExpression operandExpression = (CodeExpression)verb.Operands[i].Accept(this);

                    this.CreateOperandMethod(operandMethodName, operandExpression);

                    ctorCall.Parameters.Add(
                        new CodeObjectCreateExpression(
                            typeof(MyAss.Framework.OperandTypes.ParExpression),
                            new CodeDelegateCreateExpression(
                                new CodeTypeReference(
                                    typeof(MyAss.Framework.OperandTypes.ExpressionDelegate)
                                ),
                                new CodeTypeReferenceExpression(theClassName),
                                operandMethodName
                           )
                        )
                    );
                }
                else
                {
                    ctorCall.Parameters.Add(new CodePrimitiveExpression(null));
                }
            }

            CodeVariableDeclarationStatement varDeclaration = new CodeVariableDeclarationStatement(
                verbType,
                "verb",
                ctorCall
            );

            CodeExpressionStatement setLabelCall = new CodeExpressionStatement(
                new CodeMethodInvokeExpression(
                    new CodeVariableReferenceExpression("verb"),
                    setLabelMethodName,
                    new CodePrimitiveExpression(verb.LabelId)
                )
            );

            CodeExpressionStatement addToModelCall = new CodeExpressionStatement(
                new CodeMethodInvokeExpression(
                    new CodeVariableReferenceExpression(resultModelVarName),
                    addVerbMethodName,
                    new CodeVariableReferenceExpression("verb")
                )
            );

            // Always positive if for several statements representation
            CodeConditionStatement result = new CodeConditionStatement(
                new CodePrimitiveExpression(true),
                varDeclaration,
                setLabelCall,
                addToModelCall);

            return result;
        }

        private void CreateOperandMethod(string methodName, CodeExpression codeExpr)
        {
            CodeMemberMethod method = new CodeMemberMethod()
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Static,
                ReturnType = new CodeTypeReference(typeof(Double)),
                Name = methodName
            };

            method.Statements.Add(
                new CodeVariableDeclarationStatement(
                    typeof(Double), 
                    "result"
                )
            );

            method.Statements.Add(
                new CodeAssignStatement(
                    new CodeVariableReferenceExpression("result"),
                    codeExpr
                )
            );

            method.Statements.Add(
                new CodeMethodReturnStatement(
                    new CodeVariableReferenceExpression("result")
                )
            );

            this.theClass.Members.Add(method);
        }

        public CodeObject Visit(ASTBinaryExpression expr)
        {
            CodeBinaryOperatorExpression result = new CodeBinaryOperatorExpression(
                (CodeExpression)expr.Left.Accept(this),
                this.MapBinaryOperator(expr.Operator),
                (CodeExpression)expr.Right.Accept(this)
            );

            return result;
        }

        public CodeObject Visit(ASTLValue lval)
        {
            return new CodePrimitiveExpression(100500);
        }

        public CodeObject Visit(ASTDirectSNACall sna)
        {
            CodeMethodInvokeExpression result = new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(
                    new CodeTypeReferenceExpression(MetadataRetriever.GetBuiltinSnaType()),
                    sna.SnaId
                ),
                new CodePrimitiveExpression(sna.ActualId)
            );

            return result;
        }

        public CodeObject Visit(ASTProcedureCall call)
        {
            CodeMethodInvokeExpression result = new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(
                    new CodeTypeReferenceExpression(MetadataRetriever.GetBuiltinProceduresType()),
                    call.ProcedureId
                )
            );

            foreach (var actual in call.Actuals)
            {
                result.Parameters.Add((CodeExpression)actual.Accept(this));
            }

            return result;
        }

        public CodeObject Visit(ASTLiteral literal)
        {
            return new CodePrimitiveExpression(literal.Value);
        }

        private CodeBinaryOperatorType MapBinaryOperator(BinaryOperatorType op)
        {
            switch (op)
            {
                case BinaryOperatorType.Add:        return CodeBinaryOperatorType.Add;
                case BinaryOperatorType.Substract:  return CodeBinaryOperatorType.Subtract;
                case BinaryOperatorType.Multiply:   return CodeBinaryOperatorType.Multiply;
                case BinaryOperatorType.Divide:     return CodeBinaryOperatorType.Divide;
                case BinaryOperatorType.Modulus:    return CodeBinaryOperatorType.Modulus;
                default: throw new CompilerException("Not supported binary operator!");
            }
        } 
    }
}
