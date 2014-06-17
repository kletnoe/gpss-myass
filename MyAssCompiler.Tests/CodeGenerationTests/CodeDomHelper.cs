using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;

namespace MyAssCompiler.Tests.CodeGenerationTests
{
    public static class CodeDomHelper
    {
        public static string Print(CodeExpression expr)
        {
            if (expr is CodeBinaryOperatorExpression)
            {
                return Print(expr as CodeBinaryOperatorExpression);
            }
            else if (expr is CodePrimitiveExpression)
            {
                return Print(expr as CodePrimitiveExpression);
            }
            else if (expr is CodeMethodInvokeExpression)
            {
                return Print(expr as CodeMethodInvokeExpression);
            }
            else if (expr is CodeMethodReferenceExpression)
            {
                return Print(expr as CodeMethodReferenceExpression);
            }

            else
            {
                return "! Wrong expression type !";
            }
        }

        public static string Print(CodeMethodReferenceExpression expr)
        {
            return expr.MethodName;
        }

        public static string Print(CodeMethodInvokeExpression expr)
        {
            StringBuilder result = new StringBuilder();
            result.Append(Print(expr.Method));
            result.Append("(");

            foreach (CodeExpression par in expr.Parameters)
            {
                result.Append(Print(par));
                result.Append(", ");
            }
            result.Remove(result.Length - 2, 2);

            result.Append(")");
            return result.ToString();
        }

        public static string Print(CodeBinaryOperatorExpression expr)
        {
            StringBuilder result = new StringBuilder();
            result.Append("(");
            result.Append(Print(expr.Left));
            result.Append(Print(expr.Operator));
            result.Append(Print(expr.Right));
            result.Append(")");
            return result.ToString();
        }

        public static string Print(CodePrimitiveExpression expr)
        {
            return expr.Value.ToString();
        }

        public static string Print(CodeBinaryOperatorType op)
        {
            switch (op)
            {
                case CodeBinaryOperatorType.Add:        return "+";
                case CodeBinaryOperatorType.Subtract:   return "-";
                case CodeBinaryOperatorType.Multiply:   return "#";
                case CodeBinaryOperatorType.Divide:     return "/";
                case CodeBinaryOperatorType.Modulus:    return "\\";
                default: return "WRONGOP";
            }
        }
    }
}
