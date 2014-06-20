using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler.AST;

namespace MyAssCompiler.CodeGeneration
{
    public class CodeDomGenerationVisitor : IASTVisitor<CodeObject>
    {
        public CodeObject Visit(ASTVerb node)
        {
            throw new NotImplementedException();
        }

        public CodeObject Visit(ASTExpression astExpr)
        {
            CodeExpression expr = (CodeExpression)astExpr.Term.Accept(this);

            foreach (var add in astExpr.Additives)
            {
                CodeBinaryOperatorExpression innerExpr = (CodeBinaryOperatorExpression)add.Accept(this);
                innerExpr.Left = expr;
                expr = innerExpr;
            }

            return expr;
        }

        public CodeObject Visit(ASTAdditive additive)
        {
            CodeBinaryOperatorExpression expr = new CodeBinaryOperatorExpression();

            switch (additive.Operator)
            {
                case AddOperatorType.ADD:
                    expr.Operator = CodeBinaryOperatorType.Add;
                    break;
                case AddOperatorType.SUBSTRACT:
                    expr.Operator = CodeBinaryOperatorType.Subtract;
                    break;
                default:
                    expr.Operator = CodeBinaryOperatorType.Add;
                    break;
            }

            expr.Right = (CodeExpression)additive.Term.Accept(this);

            return expr;
        }

        public CodeObject Visit(ASTTerm node)
        {
            throw new NotImplementedException();
        }

        public CodeObject Visit(ASTActuals node)
        {
            throw new NotImplementedException();
        }

        public CodeObject Visit(ASTCall node)
        {
            throw new NotImplementedException();
        }

        public CodeObject Visit(ASTSuffixOperator node)
        {
            throw new NotImplementedException();
        }

        public CodeObject Visit(ASTLValue node)
        {
            throw new NotImplementedException();
        }

        public CodeObject Visit(ASTLiteral node)
        {
            throw new NotImplementedException();
        }

        public CodeObject Visit(ASTDirectSNA node)
        {
            throw new NotImplementedException();
        }

        public CodeObject Visit(ASTSignedFactor node)
        {
            throw new NotImplementedException();
        }

        public CodeObject Visit(ASTMultiplicative node)
        {
            throw new NotImplementedException();
        }

        public CodeObject Visit(ASTOperands node)
        {
            throw new NotImplementedException();
        }

        private static string GetClassName()
        {
            return "Model_1";
        }

        public static string GetOperandName(int blockNo, int operandNo)
        {
            return "Block" + blockNo + "_Operand" + operandNo;
        }

        public CodeObject Visit(ASTModel node)
        {
            throw new NotImplementedException();
        }

        //public CodeObject Visit(ASTModel node)
        //{
        //    int currentBlockNo = 1;

        //    CodeTypeDeclaration currentClass = new CodeTypeDeclaration()
        //    {
        //        Attributes = MemberAttributes.Public | MemberAttributes.Static,
        //        Name = GetClassName()
        //    };

        //    foreach (var verb in node.Verbs)
        //    {
        //        verb.Accept(this);
        //    }

        //    this.rootnamespace.Types.Add(this.currentClass);

        //    this.currentModelNo++;
        //}

        //public void Visit(ASTVerb node)
        //{
        //    this.currentOperandNo = 1;
        //    node.Operands.Accept(this);
        //    this.currentBlockNo++;
        //}

        //public void Visit(ASTOperands node)
        //{
        //    foreach (var operand in node.Operands)
        //    {
        //        if (operand != null)
        //        {
        //            this.currentMethod = new CodeMemberMethod()
        //            {
        //                Attributes = MemberAttributes.Public | MemberAttributes.Static,
        //                ReturnType = new CodeTypeReference(typeof(Double)),
        //                Name = this.CurrentMethodName,
        //            };

        //            CodeVariableDeclarationStatement declareVarStatement
        //                = new CodeVariableDeclarationStatement(typeof(Double), "result");

        //            CodeAssignStatement assignStatement = new CodeAssignStatement();
        //            assignStatement.Left = new CodeVariableReferenceExpression(declareVarStatement.Name);

        //            // Accept operand
        //            operand.Accept(this);

        //            assignStatement.Right = this.currentExpression;

        //            CodeMethodReturnStatement returnStatement =
        //                new CodeMethodReturnStatement(new CodeVariableReferenceExpression(declareVarStatement.Name));

        //            this.currentMethod.Statements.Add(declareVarStatement);
        //            this.currentMethod.Statements.Add(assignStatement);
        //            this.currentMethod.Statements.Add(returnStatement);

        //            this.currentClass.Members.Add(currentMethod);
        //        }

        //        this.currentOperandNo++;
        //    }
        //}

        //public void Visit(ASTExpression astExpr)
        //{
        //    if (astExpr.Additives.Count == 0)
        //    {
        //        astExpr.Term.Accept(this);
        //    }
        //    else
        //    {
        //        astExpr.Term.Accept(this);
        //        CodeExpression left = this.currentExpression;

        //        CodeBinaryOperatorExpression expr;
        //        foreach (var add in astExpr.Additives)
        //        {
        //            expr = new CodeBinaryOperatorExpression();
        //            expr.Left = left;
        //            this.currentExpression = expr;
        //            add.Accept(this);
        //            left = this.currentExpression;
        //        }
        //    }
        //}

        //public void Visit(ASTAdditive additive)
        //{
        //    CodeBinaryOperatorExpression expr = (CodeBinaryOperatorExpression)this.currentExpression;

        //    switch (additive.Operator)
        //    {
        //        case AddOperatorType.ADD:
        //            expr.Operator = CodeBinaryOperatorType.Add;
        //            break;
        //        case AddOperatorType.SUBSTRACT:
        //            expr.Operator = CodeBinaryOperatorType.Subtract;
        //            break;
        //        default:
        //            expr.Operator = CodeBinaryOperatorType.Add;
        //            break;
        //    }

        //    additive.Term.Accept(this);
        //    expr.Right = this.currentExpression;

        //    this.currentExpression = expr;
        //}

        //public void Visit(ASTTerm term)
        //{
        //    if (term.Multiplicatives.Count == 0)
        //    {
        //        term.Factor.Accept(this);
        //    }
        //    else
        //    {
        //        term.Factor.Accept(this);
        //        CodeExpression left = this.currentExpression;

        //        CodeBinaryOperatorExpression expr;
        //        foreach (var mult in term.Multiplicatives)
        //        {
        //            expr = new CodeBinaryOperatorExpression();
        //            expr.Left = left;
        //            this.currentExpression = expr;
        //            mult.Accept(this);
        //            left = this.currentExpression;
        //        }
        //    }

        //}

        //public void Visit(ASTMultiplicative mult)
        //{
        //    CodeBinaryOperatorExpression expr = (CodeBinaryOperatorExpression)this.currentExpression;

        //    switch (mult.Operator)
        //    {
        //        case MulOperatorType.MULTIPLY:
        //            expr.Operator = CodeBinaryOperatorType.Multiply;
        //            break;
        //        case MulOperatorType.DIVIDE:
        //            expr.Operator = CodeBinaryOperatorType.Divide;
        //            break;
        //        case MulOperatorType.MODULUS:
        //            expr.Operator = CodeBinaryOperatorType.Modulus;
        //            break;
        //        case MulOperatorType.EXPONENT:
        //            throw new NotImplementedException("Exponentiation operator not implemented!");
        //        default:
        //            expr.Operator = CodeBinaryOperatorType.Add;
        //            break;
        //    }

        //    mult.Factor.Accept(this);
        //    expr.Right = this.currentExpression;

        //    this.currentExpression = expr;
        //}

        //public void Visit(ASTSignedFactor astFactor)
        //{
        //    if (astFactor.Operator == null)
        //    {
        //        astFactor.Value.Accept(this);
        //    }
        //    else
        //    {
        //        CodeBinaryOperatorExpression signedFactor = new CodeBinaryOperatorExpression();
        //        signedFactor.Left = new CodePrimitiveExpression(0);

        //        switch (astFactor.Operator)
        //        {
        //            case AddOperatorType.SUBSTRACT:
        //                signedFactor.Operator = CodeBinaryOperatorType.Subtract;
        //                break;
        //            default:
        //                signedFactor.Operator = CodeBinaryOperatorType.Add;
        //                break;
        //        }

        //        astFactor.Value.Accept(this);

        //        signedFactor.Right = this.currentExpression;

        //        this.currentExpression = signedFactor;
        //    }
        //}

        //public void Visit(ASTLValue lval)
        //{
        //    if (lval.Accessor != null)
        //    {
        //        lval.Accessor.Accept(this);

        //        if (lval.Accessor is ASTDirectSNA)
        //        {
        //            CodeMethodInvokeExpression expr = new CodeMethodInvokeExpression(
        //                new CodeMethodReferenceExpression(
        //                    new CodeTypeReferenceExpression(MetadataRetriever.GetBuiltinSnaType()),
        //                    this.parser.IdsList[lval.Id]),
        //                this.currentExpression);

        //            this.currentExpression = expr;
        //        }
        //    }
        //}

        //public void Visit(ASTCall node)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Visit(ASTSuffixOperator node)
        //{
        //    throw new NotImplementedException();
        //}
        //public void Visit(ASTLiteral literal)
        //{
        //    this.currentExpression = new CodePrimitiveExpression(literal.Value);
        //}

        //public void Visit(ASTDirectSNA sna)
        //{
        //    this.currentExpression = new CodePrimitiveExpression(sna.Id);
        //}

        //public void Visit(ASTActuals node)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
