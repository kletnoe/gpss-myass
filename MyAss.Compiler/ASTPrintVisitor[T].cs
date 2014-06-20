using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAss.Compiler.AST;

namespace MyAss.Compiler
{
    class ASTPrintVisitor_T : IASTVisitor<string>
    {
        public IParser Parser { get; private set; }

        public ASTPrintVisitor_T(IParser parser)
        {
            this.Parser = parser;
        }

        public string Print()
        {
            return this.Visit(this.Parser.Parse());
        }

        public string Visit(ASTModel model)
        {
            StringBuilder result = new StringBuilder();

            foreach (var verb in model.Verbs)
            {
                result.Append(verb.Accept(this));
                result.Append(Environment.NewLine);
            }

            return result.ToString();
        }

        public string Visit(ASTVerb verb)
        {
            StringBuilder result = new StringBuilder();

            if (verb != null)
            {
                result.Append(verb.LabelId);
                result.Append(" ");
            }

            result.Append(verb.VerbId);
            result.Append(" ");

            if (verb.Operands != null)
            {
                result.Append(verb.Operands.Accept(this));
            }

            return result.ToString();
        }

        public string Visit(ASTOperands operands)
        {
            StringBuilder result = new StringBuilder();

            if (operands.Operands.Count > 0)
            {
                foreach (var actual in operands.Operands)
                {
                    result.Append(actual.Accept(this));
                    result.Append(", ");
                }
                result.Remove(result.Length - 2, 2);
            }

            return result.ToString();
        }

        public string Visit(ASTExpression expr)
        {
            StringBuilder result = new StringBuilder();

            //result.Append("(");
            result.Append(expr.Term.Accept(this));

            foreach (var additive in expr.Additives)
            {
                result.Append(additive.Accept(this));
            }
            //result.Append(")");

            return result.ToString();
        }

        public string Visit(ASTAdditive additive)
        {
            StringBuilder result = new StringBuilder();

            switch (additive.Operator)
            {
                case AddOperatorType.ADD:
                    result.Append("+");
                    break;
                case AddOperatorType.SUBSTRACT:
                    result.Append("-");
                    break;
            }

            result.Append(additive.Term.Accept(this));

            return result.ToString();
        }

        public string Visit(ASTTerm term)
        {
            StringBuilder result = new StringBuilder();

            result.Append(term.Factor.Accept(this));

            foreach (var mult in term.Multiplicatives)
            {
                result.Append(mult.Accept(this));
            }

            return result.ToString();
        }

        public string Visit(ASTMultiplicative mult)
        {
            StringBuilder result = new StringBuilder();

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

            result.Append(mult.Factor.Accept(this));

            if (mult.Factor is ASTExpression)
            {
                result.Append(")");
            }

            return result.ToString();
        }

        public string Visit(ASTLiteral literal)
        {
            return literal.Value.ToString();
        }

        public string Visit(ASTLValue lval)
        {
            StringBuilder result = new StringBuilder();

            result.Append(lval.Id);
            if (lval.Accessor != null)
            {
                result.Append(lval.Accessor.Accept(this));
            }

            return result.ToString();
        }

        public string Visit(ASTDirectSNA sna)
        {
            StringBuilder result = new StringBuilder();

            result.Append("$");
            result.Append(sna.Id);

            return result.ToString();
        }

        public string Visit(ASTCall call)
        {
            StringBuilder result = new StringBuilder();

            result.Append("(");

            if (call.Actuals != null)
            {
                result.Append(call.Actuals.Accept(this));
            }

            result.Append(")");

            return result.ToString();
        }

        public string Visit(ASTActuals actuals)
        {
            StringBuilder result = new StringBuilder();

            if (actuals.Expressions.Count > 0)
            {
                foreach (var actual in actuals.Expressions)
                {
                    result.Append(actual.Accept(this));
                    result.Append(",");
                }
                result.Remove(result.Length - 2, 2);
            }

            return result.ToString();
        }

        public string Visit(ASTSignedFactor factor)
        {
            StringBuilder result = new StringBuilder();

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

            result.Append(factor.Value.Accept(this));

            if (factor.Value is ASTExpression)
            {
                result.Append(")");
            }

            return result.ToString();
        }

        public string Visit(ASTSuffixOperator op)
        {
            switch (op.Operator)
            {
                case AddOperatorType.ADD:
                    return "+";
                case AddOperatorType.SUBSTRACT:
                    return "-";
                default: return string.Empty;
            }
        }
    }
}
