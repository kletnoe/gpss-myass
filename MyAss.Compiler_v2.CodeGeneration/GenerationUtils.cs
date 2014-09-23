using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using Microsoft.CSharp.RuntimeBinder;
using MyAss.Compiler_v2.AST;

namespace MyAss.Compiler_v2.CodeGeneration
{
    public static class GenerationUtils
    {
        private const string SetIdMethodName = "SetId";
        private const string AddVerbMethodName = "AddVerb";
        private const string AddNameMethodName = "AddName";
        private const string ReplaceNameIdMethodName = "ReplaceNameId";

        public static CodeBinaryOperatorType MapBinaryOperator(BinaryOperatorType op)
        {
            switch (op)
            {
                case BinaryOperatorType.Add: return CodeBinaryOperatorType.Add;
                case BinaryOperatorType.Substract: return CodeBinaryOperatorType.Subtract;
                case BinaryOperatorType.Multiply: return CodeBinaryOperatorType.Multiply;
                case BinaryOperatorType.Divide: return CodeBinaryOperatorType.Divide;
                case BinaryOperatorType.Modulus: return CodeBinaryOperatorType.Modulus;
                default: throw new CompilerException("Not supported binary operator!");
            }
        }

        public static CodeMemberField ConstructCreateNamedVar(string namedVarName)
        {
            CodeMemberField field = new CodeMemberField();
            field.Attributes = MemberAttributes.Private;
            field.Type = new CodeTypeReference(typeof(int));
            field.Name = namedVarName;

            return field;
        }

        public static CodeAssignStatement ConstructAssignNamedVar(string namedVarName, int value)
        {
            CodeAssignStatement assign = new CodeAssignStatement();
            assign.Left = new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                namedVarName
            );
            assign.Right = new CodePrimitiveExpression(value);

            return assign;
        }

        public static CodeExpressionStatement ConstructCallAddNameMethod(string namedVarName)
        {
            CodeMethodInvokeExpression methodCall = new CodeMethodInvokeExpression();
            methodCall.Method = new CodeMethodReferenceExpression();
            methodCall.Method.TargetObject = new CodeThisReferenceExpression();
            methodCall.Method.MethodName = AddNameMethodName;

            methodCall.Parameters.Add(new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                namedVarName
            ));
            methodCall.Parameters.Add(new CodePrimitiveExpression(namedVarName));

            return new CodeExpressionStatement(methodCall);
        }


        public static CodeExpressionStatement ConstructCallReplaceNameIdMethod(string namedVarName)
        {
            CodeMethodInvokeExpression methodCall = new CodeMethodInvokeExpression();
            methodCall.Method = new CodeMethodReferenceExpression();
            methodCall.Method.TargetObject = new CodeThisReferenceExpression();
            methodCall.Method.MethodName = ReplaceNameIdMethodName;

            methodCall.Parameters.Add(new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                namedVarName
            ));
            methodCall.Parameters.Add(new CodePrimitiveExpression(namedVarName));

            return new CodeExpressionStatement(methodCall);
        }

        public static CodeExpressionStatement ConstructCallCommandSetIdMethod(string verbVarName, string namedVarName)
        {
            CodeMethodInvokeExpression methodCall = new CodeMethodInvokeExpression();
            methodCall.Method = new CodeMethodReferenceExpression();
            methodCall.Method.TargetObject = new CodeVariableReferenceExpression(verbVarName);
            methodCall.Method.MethodName = SetIdMethodName;

            methodCall.Parameters.Add(new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                namedVarName
            ));

            return new CodeExpressionStatement(methodCall);
        }

        public static CodeExpressionStatement ConstructCallAddVerbMethod(string verbVarName)
        {
            CodeMethodInvokeExpression methodCall = new CodeMethodInvokeExpression();
            methodCall.Method = new CodeMethodReferenceExpression();
            methodCall.Method.TargetObject = new CodeThisReferenceExpression();
            methodCall.Method.MethodName = AddVerbMethodName;

            methodCall.Parameters.Add(new CodeVariableReferenceExpression(verbVarName));

            return new CodeExpressionStatement(methodCall);
        }

        public static CodeObjectCreateExpression ConctructCtorDelegateParameter(string parameterMethodName)
        {
            return new CodeObjectCreateExpression(
                typeof(MyAss.Framework_v2.OperandTypes.ParExpression),
                new CodeDelegateCreateExpression(
                    new CodeTypeReference(
                        typeof(MyAss.Framework_v2.OperandTypes.ExpressionDelegate)
                    ),
                    new CodeThisReferenceExpression(),
                    parameterMethodName
                )
            );
        }

        public static CodeObjectCreateExpression ConstructCtorLiteralParameter(string literal)
        {
            return new CodeObjectCreateExpression(
                typeof(MyAss.Framework_v2.OperandTypes.LiteralOperand),
                new CodePrimitiveExpression(literal)
            );
        }


        // Generates:
        //public double %snaName%(int entityId)
        //{
        //    try
        //    {
        //        IEntity entity = simulation.GetEntity(entityId);
        //        dynamic dynamicEntity = entity;
        //        return (double)dynamicEntity.%snaName%();
        //    }
        //    catch (RuntimeBinderException)
        //    {
        //        return 0;
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        return 0;
        //    }
        //}
        public static CodeMemberMethod ConstructSimpleSnaMethod(string snaName)
        {
            CodeMemberMethod method = new CodeMemberMethod();
            method.Attributes = MemberAttributes.Private;
            method.ReturnType = new CodeTypeReference(typeof(double));
            method.Name = snaName;

            method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "entityId"));

            CodeTryCatchFinallyStatement tryCatchStatement = new CodeTryCatchFinallyStatement();

            tryCatchStatement.TryStatements.Add(new CodeSnippetStatement("var entity = this.simulation.GetEntity(entityId);"));
            tryCatchStatement.TryStatements.Add(new CodeSnippetStatement("dynamic dynamicEntity = entity;"));
            tryCatchStatement.TryStatements.Add(new CodeSnippetStatement("return (double)dynamicEntity." + snaName + "();"));


            CodeCatchClause catchRuntimeBinderException = new CodeCatchClause();
            catchRuntimeBinderException.CatchExceptionType = new CodeTypeReference(typeof(RuntimeBinderException));
            catchRuntimeBinderException.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(0D)));

            CodeCatchClause catchKeyNotFoundException = new CodeCatchClause();
            catchKeyNotFoundException.CatchExceptionType = new CodeTypeReference(typeof(KeyNotFoundException));
            catchKeyNotFoundException.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(0D)));

            tryCatchStatement.CatchClauses.Add(catchRuntimeBinderException);
            tryCatchStatement.CatchClauses.Add(catchKeyNotFoundException);

            method.Statements.Add(tryCatchStatement);

            return method;
        }

        public static string PrintCodeObject(CodeCompileUnit compileUnit)
        {
            CodeGeneratorOptions options = new CodeGeneratorOptions()
            {

            };

            StringWriter writer = new StringWriter();

            CSharpCodeProvider provider = new CSharpCodeProvider();
            provider.GenerateCodeFromCompileUnit(compileUnit, writer, options);

            writer.Flush();
            return writer.ToString();
        }
    }
}
