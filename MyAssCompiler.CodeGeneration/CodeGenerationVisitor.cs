﻿using System;
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

        private const string namespaceName = "Modeling";

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
            theAssembly.ReferencedAssemblies.Add("MyAssFramework.dll");

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

            foreach (var verb in node.Verbs)
            {
                verb.Accept(this);
            }

            this.rootnamespace.Types.Add(this.currentClass);

            this.currentModelNo++;
        }

        public void Visit(ASTVerb node)
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

                    this.currentClass.Members.Add(currentMethod);
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
                            this.parser.IdsList[lval.Id]),
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
