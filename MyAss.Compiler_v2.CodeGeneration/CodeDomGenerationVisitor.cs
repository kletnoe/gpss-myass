using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using MyAss.Compiler.Metadata;
using MyAss.Compiler_v2.AST;
using MyAss.Framework_v2;
using MyAss.Framework_v2.Blocks;
using MyAss.Framework_v2.Commands;

namespace MyAss.Compiler_v2.CodeGeneration
{
    public class CodeDomGenerationVisitor : IASTVisitor<CodeObject>
    {
        private const string theNamespaceName = "Modeling";
        private const string theClassName = "TheModel";

        private const string setIdMethodName = "SetId";
        private const string addVerbMethodName = "AddVerb";
        private const string addNameMethodName = "AddName";
        private const string replaceNameIdMethodName = "ReplaceNameId";

        private const string simulationFieldName = "simulation";


        private Parser_v2 parser;
        private MetadataRetriever_v2 metadataRetriever;
        private Dictionary<string, int> namedVars = new Dictionary<string, int>();

        CodeNamespace theNamespace;
        CodeTypeDeclaration theClass;
        CodeConstructor theConstructor;

        private int currentBlockNo = 1;
        private int currentCommandNo = 1;
        private int verbNo = 1;
        private int currentNamedVarNo = 10000;

        public CodeDomGenerationVisitor(Parser_v2 parser)
        {
            this.parser = parser;
            this.metadataRetriever = parser.MetadataRetriever;

            this.theNamespace = new CodeNamespace();
            this.theNamespace.Name = CodeDomGenerationVisitor.theNamespaceName;

            this.theClass = new CodeTypeDeclaration();
            this.theClass.Attributes = MemberAttributes.Public;
            this.theClass.Name = CodeDomGenerationVisitor.theClassName;
            this.theClass.BaseTypes.Add(typeof(AbstractModel));
            this.theNamespace.Types.Add(this.theClass);

            this.theConstructor = new CodeConstructor();
            this.theConstructor.Attributes = MemberAttributes.Public;
            this.theClass.Members.Add(this.theConstructor);
        }

        public CodeCompileUnit VisitAll()
        {
            ASTModel model = parser.Parse();

            model.Accept(this);

            CodeCompileUnit theAssembly = new CodeCompileUnit();
            theAssembly.ReferencedAssemblies.AddRange(this.metadataRetriever.AsssemblyPaths.ToArray());
            theAssembly.Namespaces.Add(theNamespace);

            return theAssembly;
        }

        public CodeObject Visit(ASTModel model)
        {
            // Verbs
            foreach (var verb in model.Verbs)
            {
                this.theConstructor.Statements.Add((CodeStatement)verb.Accept(this));
                this.verbNo++;
            }

            return this.theClass;
        }

        public CodeObject Visit(ASTVerb verb)
        {
            Type verbType = this.metadataRetriever.GetVerb(verb.VerbId);

            // Construct always true if block to wrap several statements
            CodeConditionStatement result = new CodeConditionStatement();
            result.Condition = new CodePrimitiveExpression(true);

            // Process label if exists
            if (verb.LabelId != null)
            {
                // If namedVar is not exists already
                if (!this.namedVars.ContainsKey(verb.LabelId))
                {
                    this.CreateNewNamedVar(verb.LabelId);
                }

                // verb is Block
                if (typeof(IBlock).IsAssignableFrom(verbType))
                {
                    this.ReplaceExistingNamedVar(verb.LabelId);
                }
            }

            // Construct verb declaration
            CodeVariableDeclarationStatement verbDeclaration = new CodeVariableDeclarationStatement();
            verbDeclaration.Type = new CodeTypeReference(verbType);
            verbDeclaration.Name = "verb";
            verbDeclaration.InitExpression = this.CreateConstructorCallExpression(verbType, verb.Operands);
            result.TrueStatements.Add(verbDeclaration);

            // Construct Set label Id for Commands
            if (verb.LabelId != null)
            {
                if (typeof(ICommand).IsAssignableFrom(verbType))
                {
                    result.TrueStatements.Add(this.CallCommandSetIdMethod("verb", verb.LabelId));
                }
            }


            // Construct AddVerb method call
            result.TrueStatements.Add(this.CallAddVerbMethod("verb"));

            return result;
        }

        private void CreateNewNamedVar(string namedVarName)
        {
            int namedVarId = this.currentNamedVarNo;
            this.namedVars.Add(namedVarName, this.currentNamedVarNo);
            this.currentNamedVarNo++;

            // Construct NamedVar field
            this.theClass.Members.Add(this.CreateNamedVar(namedVarName));

            // Assign namedVar
            this.theConstructor.Statements.Add(this.AssignNamedVar(namedVarName, namedVarId));
            this.theConstructor.Statements.Add(this.CallAddNameMethod(namedVarName));
        }

        private void ReplaceExistingNamedVar(string namedVarName)
        {
            int blockNo = this.currentBlockNo;
            this.theConstructor.Statements.Add(this.AssignNamedVar(namedVarName, blockNo));
            this.theConstructor.Statements.Add(this.CallReplaceNameIdMethod(namedVarName));
            this.theConstructor.Statements.Add(this.CallAddNameMethod(namedVarName));
        }

        private CodeMemberField CreateNamedVar(string namedVarName)
        {
            CodeMemberField field = new CodeMemberField();
            field.Attributes = MemberAttributes.Private;
            field.Type = new CodeTypeReference(typeof(int));
            field.Name = namedVarName;

            return field;
        }

        private CodeAssignStatement AssignNamedVar(string namedVarName, int value)
        {
            CodeAssignStatement assign = new CodeAssignStatement();
            assign.Left = new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                namedVarName
            );
            assign.Right = new CodePrimitiveExpression(value);

            return assign;
        }

        private CodeExpressionStatement CallAddNameMethod(string namedVarName)
        {
            CodeMethodInvokeExpression methodCall = new CodeMethodInvokeExpression();
            methodCall.Method = new CodeMethodReferenceExpression();
            methodCall.Method.TargetObject = new CodeThisReferenceExpression();
            methodCall.Method.MethodName = CodeDomGenerationVisitor.addNameMethodName;

            methodCall.Parameters.Add(new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                namedVarName
            ));
            methodCall.Parameters.Add(new CodePrimitiveExpression(namedVarName));

            return new CodeExpressionStatement(methodCall);
        }


        private CodeExpressionStatement CallReplaceNameIdMethod(string namedVarName)
        {
            CodeMethodInvokeExpression methodCall = new CodeMethodInvokeExpression();
            methodCall.Method = new CodeMethodReferenceExpression();
            methodCall.Method.TargetObject = new CodeThisReferenceExpression();
            methodCall.Method.MethodName = CodeDomGenerationVisitor.replaceNameIdMethodName;

            methodCall.Parameters.Add(new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                namedVarName
            ));
            methodCall.Parameters.Add(new CodePrimitiveExpression(namedVarName));

            return new CodeExpressionStatement(methodCall);
        }

        private CodeExpressionStatement CallCommandSetIdMethod(string verbVarName, string namedVarName)
        {
            CodeMethodInvokeExpression methodCall = new CodeMethodInvokeExpression();
            methodCall.Method = new CodeMethodReferenceExpression();
            methodCall.Method.TargetObject = new CodeVariableReferenceExpression(verbVarName);
            methodCall.Method.MethodName = CodeDomGenerationVisitor.setIdMethodName;

            methodCall.Parameters.Add(new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                namedVarName
            ));

            return new CodeExpressionStatement(methodCall);
        }

        private CodeExpressionStatement CallAddVerbMethod(string verbVarName)
        {
            CodeMethodInvokeExpression methodCall = new CodeMethodInvokeExpression();
            methodCall.Method = new CodeMethodReferenceExpression();
            methodCall.Method.TargetObject = new CodeThisReferenceExpression();
            methodCall.Method.MethodName = CodeDomGenerationVisitor.addVerbMethodName;

            methodCall.Parameters.Add(new CodeVariableReferenceExpression(verbVarName));

            return new CodeExpressionStatement(methodCall);
        }

        private CodeObjectCreateExpression CreateConstructorCallExpression(Type verbType, IList<IASTExpression> operands)
        {
            int verbCtorParamsCount = verbType.GetConstructors().First().GetParameters().Length;

            CodeObjectCreateExpression ctorCall = new CodeObjectCreateExpression(verbType);

            for (int i = 0; i < verbCtorParamsCount; i++)
            {
                if (operands.Count > i && operands[i] != null)
                {
                    string operandMethodName = String.Format("Verb{0}_Operand{1}", this.verbNo, i + 1);
                    CodeExpression operandExpression = (CodeExpression)operands[i].Accept(this);

                    this.CreateOperandMethod(operandMethodName, operandExpression);

                    ctorCall.Parameters.Add(
                        new CodeObjectCreateExpression(
                            typeof(MyAss.Framework_v2.OperandTypes.ParExpression),
                            new CodeDelegateCreateExpression(
                                new CodeTypeReference(
                                    typeof(MyAss.Framework_v2.OperandTypes.ExpressionDelegate)
                                ),
                                new CodeThisReferenceExpression(),
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

            return ctorCall;
        }

        private void CreateOperandMethod(string methodName, CodeExpression codeExpr)
        {
            CodeMemberMethod method = new CodeMemberMethod()
            {
                Attributes = MemberAttributes.Public,
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
            // If namedVar is not exists already
            if (!this.namedVars.ContainsKey(lval.Id))
            {
                this.CreateNewNamedVar(lval.Id);
            }

            return new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                lval.Id
            );
        }

        public CodeObject Visit(ASTDirectSNACall sna)
        {
            // If namedVar is not exists already
            if (!this.namedVars.ContainsKey(sna.ActualId))
            {
                this.CreateNewNamedVar(sna.ActualId);
            }


            CodeMethodInvokeExpression result = new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(
                    new CodeTypeReferenceExpression(this.metadataRetriever.GetProcedure(sna.SnaId).ReflectedType),
                    sna.SnaId
                ),
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    CodeDomGenerationVisitor.simulationFieldName
                ),
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(),
                    sna.ActualId
                )
            );

            return result;
        }

        public CodeObject Visit(ASTProcedureCall call)
        {
            CodeMethodInvokeExpression result = new CodeMethodInvokeExpression(
                new CodeMethodReferenceExpression(
                    new CodeTypeReferenceExpression(this.metadataRetriever.GetProcedure(call.ProcedureId).ReflectedType),
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
