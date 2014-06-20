using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Compiler.AST;

namespace MyAss.Compiler
{
    public class ASTPrintVisitor : IASTVisitor
    {
        public IParser Parser { get; private set; }

        private StringBuilder result;

        public ASTPrintVisitor(IParser parser)
        {
            this.Parser = parser;
        }

        public string Print()
        {
            result = new StringBuilder();
            this.Visit(this.Parser.Parse());
            return result.ToString();
        }


        public void Visit(ASTModel model)
        {
            foreach (var verb in model.Verbs)
            {
                verb.Accept(this);
                result.Append(Environment.NewLine);
            }
        }

        public void Visit(ASTVerb verb)
        {
            if (verb != null)
            {
                result.Append(verb.LabelId);
                result.Append(" ");
            }

            result.Append(verb.VerbId);
            result.Append(" ");

            if (verb.Operands != null)
            {
                verb.Operands.Accept(this);
            }
        }

        public void Visit(ASTOperands operands)
        {
            if (operands.Operands.Count > 0)
            {
                operands.Operands[0].Accept(this);
                foreach (var actual in operands.Operands.Skip(1))
                {
                    result.Append(",");
                    actual.Accept(this);
                }
            }
        }

        public void Visit(ASTExpression expr)
        {
            //result.Append("(");
            expr.Term.Accept(this);

            foreach (var additive in expr.Additives)
            {
                additive.Accept(this);
            }
            //result.Append(")");
        }

        public void Visit(ASTAdditive additive)
        {
            switch (additive.Operator)
            {
                case AddOperatorType.ADD:
                    result.Append("+");
                    break;
                case AddOperatorType.SUBSTRACT:
                    result.Append("-");
                    break;
            }

            additive.Term.Accept(this);
        }

        public void Visit(ASTTerm term)
        {
            term.Factor.Accept(this);

            foreach (var mult in term.Multiplicatives)
            {
                mult.Accept(this);
            }
        }

        public void Visit(ASTMultiplicative mult)
        {
            switch (mult.Operator)
            {
                case MulOperatorType.MULTIPLY:
                    result.Append("#");
                    break;
                case MulOperatorType.DIVIDE:
                    result.Append("/");
                    break;
                case MulOperatorType.MODULUS:
                    result.Append("\\");
                    break;
                case MulOperatorType.EXPONENT:
                    result.Append("^");
                    break;
            }

            if (mult.Factor is ASTExpression)
            {
                result.Append("(");
            }

            mult.Factor.Accept(this);

            if (mult.Factor is ASTExpression)
            {
                result.Append(")");
            }
        }

        public void Visit(ASTLiteral literal)
        {
            result.Append(literal.Value);
        }

        public void Visit(ASTLValue lval)
        {
            result.Append(lval.Id);
            if (lval.Accessor != null)
            {
                lval.Accessor.Accept(this);
            }
        }

        public void Visit(ASTDirectSNA sna)
        {
            result.Append("$");
            result.Append(sna.Id);
        }

        public void Visit(ASTCall call)
        {
            result.Append("(");

            if (call.Actuals != null)
            {
                call.Actuals.Accept(this);
            }

            result.Append(")");
        }

        public void Visit(ASTActuals actuals)
        {
            if (actuals.Expressions.Count > 0)
            {
                actuals.Expressions[0].Accept(this);
                foreach (var actual in actuals.Expressions.Skip(1))
                {
                    result.Append(",");
                    actual.Accept(this);
                }
            }
        }

        public void Visit(ASTSignedFactor factor)
        {
            if (factor.Operator.HasValue)
            {
                switch (factor.Operator)
                {
                    case AddOperatorType.ADD:
                        result.Append("+");
                        break;
                    case AddOperatorType.SUBSTRACT:
                        result.Append("-");
                        break;
                }
            }

            if (factor.Value is ASTExpression)
            {
                result.Append("(");
            }

            factor.Value.Accept(this);

            if (factor.Value is ASTExpression)
            {
                result.Append(")");
            }
        }

        public void Visit(ASTSuffixOperator op)
        {
            switch (op.Operator)
            {
                case AddOperatorType.ADD:
                    result.Append("+");
                    break;
                case AddOperatorType.SUBSTRACT:
                    result.Append("-");
                    break;
            }
        }
    }
}
