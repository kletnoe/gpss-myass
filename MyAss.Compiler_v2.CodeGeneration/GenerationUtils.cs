using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
