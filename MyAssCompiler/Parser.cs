using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler.AST;
using MyAssCompiler.Metadata;

namespace MyAssCompiler
{
    public class Parser : IParser
    {
        public IScanner Scanner { get; private set; }

        public Parser(IScanner scanner)
        {
            this.Scanner = scanner;
        }

        public ASTModel Parse()
        {
            return this.ExpectModel();
        }

        public string ExpectID()
        {
            string id = (string)this.Expect(TokenType.ID);
            return id;
        }

        // Single token
        public object Expect(TokenType expected)
        {
            if (expected == this.Scanner.CurrentToken)
            {
                // TODO: int -> object or something
                object res = this.Scanner.CurrentTokenVal;
                this.Scanner.Next();

                return res;
            }
            else
            {
                throw new Exception(String.Format("Expected {0} but got {1} at line {2} column {3}",
                    expected, Scanner.CurrentToken, Scanner.CurrentTokenLine, Scanner.CurrentTokenColumn));
            }
        }


        // <model> ::= { <verb> [ COMMENT ] "\r\n" }+
        public ASTModel ExpectModel()
        {
             ASTModel model = new ASTModel();

            while (this.Scanner.CurrentToken != TokenType.EOF)
            {
                // Eat comments, white;
                while (this.Scanner.CurrentToken == TokenType.COMMENT
                    || this.Scanner.CurrentToken == TokenType.WHITE)
                {
                    switch (this.Scanner.CurrentToken)
                    {
                        case TokenType.COMMENT:
                            this.Expect(TokenType.COMMENT);
                            break;
                        case TokenType.WHITE:
                            this.Expect(TokenType.WHITE);
                            break;
                    }
                }

                if (this.Scanner.CurrentToken == TokenType.ID)
                {
                    model.Verbs.Add(ExpectVerb());
                }

                // Eat comment
                if (this.Scanner.CurrentToken == TokenType.COMMENT)
                {
                    this.Expect(TokenType.COMMENT);
                }

                this.Expect(TokenType.LF);
            }

            return model;
        }

        // <verb> ::= [ ID ] ID [ <operands> ]
        public ASTVerb ExpectVerb()
        {
            ASTVerb verb = new ASTVerb();

            string firstId = this.ExpectID();
            if (MetadataRetriever.IsBuiltinVerb(firstId))
            {
                // Id is verb 
                verb.VerbId = firstId;

                do
                {
                    this.Expect(TokenType.WHITE);
                } while (this.Scanner.CurrentToken == TokenType.WHITE);

                verb.Operands = this.ExpectOperands();
            }
            else
            {
                // Id is label
                verb.LabelId = firstId;

                do
                {
                    this.Expect(TokenType.WHITE);
                } while (this.Scanner.CurrentToken == TokenType.WHITE);

                string secondId = this.ExpectID();

                verb.VerbId = secondId;

                do
                {
                    this.Expect(TokenType.WHITE);
                } while (this.Scanner.CurrentToken == TokenType.WHITE);

                verb.Operands = this.ExpectOperands();
            }
            

            Console.WriteLine(verb);
            return verb;
        }

        public ASTOperands ExpectOperands()
        {
            ASTOperands operands = new ASTOperands();

            operands.Operands.Add(this.ExpectOperand());

            while (this.Scanner.CurrentToken == TokenType.COMMA
                    || this.Scanner.CurrentToken == TokenType.WHITE)
            {
                switch (this.Scanner.CurrentToken)
                {
                    case TokenType.COMMA:
                        this.Expect(TokenType.COMMA);
                        break;
                    case TokenType.WHITE:
                        this.Expect(TokenType.WHITE);
                        break;
                    default:
                        throw new Exception(String.Format("Expected {0} but got {1} at line {2} column {3}",
                   @"WhiteSpace or Comma", Scanner.CurrentToken, Scanner.CurrentTokenLine, Scanner.CurrentTokenColumn));
                }

                operands.Operands.Add(this.ExpectOperand());
            }

            return operands;
        }

        // <operand> ::= "" | <expression> | <parexpression>
        public ASTOperand ExpectOperand()
        {
            ASTOperand operand = null;

            if(this.Scanner.CurrentToken == TokenType.LPAR)
            {
                operand = this.ExpectParExpression();
            }
            else if (this.Scanner.CurrentToken == TokenType.ID
                || this.Scanner.CurrentToken == TokenType.NUMERIC
                || this.Scanner.CurrentToken == TokenType.MINUS
                || this.Scanner.CurrentToken == TokenType.PLUS)
            {
                operand = this.ExpectExpression();
            }

            return operand;
        }

        private ASTOperand ExpectParExpression()
        {
            ASTOperand operand = null;

            this.Scanner.IgnoreWhitespace = true;
            this.Expect(TokenType.LPAR);

            operand = ExpectExpression();

            this.Scanner.IgnoreWhitespace = false;
            this.Expect(TokenType.RPAR);

            return operand;
        }


        // <expression> ::= <term> { <additive> }
        public ASTExpression ExpectExpression()
        {
            ASTExpression expression = new ASTExpression();

            expression.Term = this.ExpectTerm();

            while (this.Scanner.CurrentToken == TokenType.PLUS
                || this.Scanner.CurrentToken == TokenType.MINUS)
            {
                expression.Additives.Add(this.ExpectAdditive());
            }

            return expression;
        }

        // <additive> ::= <addop> <term>
        public ASTAdditive ExpectAdditive()
        {
            ASTAdditive additive = new ASTAdditive();

            additive.Operator = this.ExpectAddOperator();
            additive.Term = this.ExpectTerm();

            return additive;
        }

        // <term> ::= ( <factor> | <signedfactor> ) { <multiplicative> }
        public ASTTerm ExpectTerm()
        {
            ASTTerm term = new ASTTerm();

            term.Factor = this.ExpectSignedFactor();

            while (this.Scanner.CurrentToken == TokenType.OCTOTHORPE
                || this.Scanner.CurrentToken == TokenType.FWDSLASH
                || this.Scanner.CurrentToken == TokenType.BCKSLASH
                || this.Scanner.CurrentToken == TokenType.CARRET)
            {
                term.Multiplicatives.Add(this.ExpectMultiplicative());
            }

            return term;
        }

        public ASTMultiplicative ExpectMultiplicative()
        {
            ASTMultiplicative mult = new ASTMultiplicative();

            mult.Operator = this.ExpectMulOperator();
            mult.Factor = this.ExpectFactor();

            return mult;
        }


        // <signedfactor> ::= [ <addop> ] <factor>
        public ASTSignedFactor ExpectSignedFactor()
        {
            ASTSignedFactor factor = new ASTSignedFactor();

            if (this.Scanner.CurrentToken == TokenType.PLUS
                || this.Scanner.CurrentToken == TokenType.MINUS)
            {
                factor.Operator = this.ExpectAddOperator();
            }
            factor.Value = ExpectFactor();

            return factor;
        }

        // <factor> ::= <literal> | <lval> | "(" <expression> ")"
        public IASTFactor ExpectFactor()
        {
            IASTFactor factor = null;

            switch (this.Scanner.CurrentToken)
            {
                case TokenType.ID:
                    factor = this.ExpectLValue();
                    break;
                case TokenType.NUMERIC:
                    factor = this.ExpectLiteral();
                    break;
                case TokenType.LPAR:
                    this.Expect(TokenType.LPAR);
                    factor = ExpectExpression();
                    this.Expect(TokenType.RPAR);
                    break;
                default:
                    throw new Exception(String.Format("Expected {0} but got {1} at line {2} column {3}",
                        @"ID or NUMERIC or '(' ", Scanner.CurrentToken, Scanner.CurrentTokenLine, Scanner.CurrentTokenColumn));
            }

            return factor;
        }


        // <literal> ::= INT | DOUBLE | STRING
        public ASTLiteral ExpectLiteral()
        {
            object value = this.Expect(TokenType.NUMERIC);
            LiteralType type;

            if (value is Int32)
            {
                type = LiteralType.Int32;
            }
            else if (value is Double)
            {
                type = LiteralType.Double;
            }
            else
            {
                type = LiteralType.String;
            }

            return new ASTLiteral()
            {
                LiteralType = type,
                Value = value
            };
        }

        // <lval> ::= ID [ <accessor> ]
        public ASTLValue ExpectLValue()
        {
            ASTLValue lvalue = new ASTLValue();

            lvalue.Id = this.ExpectID();

            if (this.Scanner.CurrentToken == TokenType.LPAR
                || this.Scanner.CurrentToken == TokenType.DOLLAR)
            {
                lvalue.Accessor = this.ExpectAccessor();
            }

            return lvalue;
        }

        // <accessor> ::= <call> | <directsna>
        public IASTAccessor ExpectAccessor()
        {
            switch (this.Scanner.CurrentToken)
            {
                case TokenType.LPAR:
                    return this.ExpectCall();
                case TokenType.DOLLAR:
                    return this.ExpectDirectSNA();
                default:
                    throw new Exception(String.Format("Expected {0} but got {1} at line {2} column {3}",
                        @"( or $ or + or -", Scanner.CurrentToken, Scanner.CurrentTokenLine, Scanner.CurrentTokenColumn));
            }
        }

        // <call> ::= "(" <actuals> ")"
        public ASTCall ExpectCall()
        {
            ASTCall call = new ASTCall();

            this.Expect(TokenType.LPAR);
            call.Actuals = this.ExpectActuals();
            this.Expect(TokenType.RPAR);

            return call;
        }

        // <actuals> ::= "" | <expr> { "," <expr> }
        public ASTActuals ExpectActuals()
        {
            ASTActuals actuals = new ASTActuals();

            if (this.Scanner.CurrentToken == TokenType.ID
                || this.Scanner.CurrentToken == TokenType.NUMERIC)
            {
                actuals.Expressions.Add(this.ExpectExpression());

                while (this.Scanner.CurrentToken == TokenType.COMMA)
                {
                    this.Expect(TokenType.COMMA);
                    actuals.Expressions.Add(this.ExpectExpression());
                }
            }

            return actuals;
        }

        // <directsna> ::= "$" ID
        public ASTDirectSNA ExpectDirectSNA()
        {
            this.Expect(TokenType.DOLLAR);
            return new ASTDirectSNA()
            {
                Id = ExpectID()
            };
        }

        // <addop> ::= "+" | "-"
        public AddOperatorType ExpectAddOperator()
        {
            switch (this.Scanner.CurrentToken)
            {
                case TokenType.PLUS:
                    this.Expect(TokenType.PLUS);
                    return AddOperatorType.ADD;
                case TokenType.MINUS:
                    this.Expect(TokenType.MINUS);
                    return AddOperatorType.SUBSTRACT;
                default:
                    throw new Exception(String.Format("Expected {0} but got {1} at line {2} column {3}",
                        @"+ or -", Scanner.CurrentToken, Scanner.CurrentTokenLine, Scanner.CurrentTokenColumn));
            }
        }

        // <mulop> ::= "#" | "/" | "%" | "^"
        public MulOperatorType ExpectMulOperator()
        {
            switch (this.Scanner.CurrentToken)
            {
                case TokenType.OCTOTHORPE:
                    this.Expect(TokenType.OCTOTHORPE);
                    return MulOperatorType.MULTIPLY;
                case TokenType.FWDSLASH:
                    this.Expect(TokenType.FWDSLASH);
                    return MulOperatorType.DIVIDE;
                case TokenType.BCKSLASH:
                    this.Expect(TokenType.BCKSLASH);
                    return MulOperatorType.MODULUS;
                case TokenType.CARRET:
                    this.Expect(TokenType.CARRET);
                    return MulOperatorType.EXPONENT;
                default:
                    throw new Exception(String.Format("Expected {0} but got {1} at line {2} column {3}",
                        @"# or / or \ or ^", Scanner.CurrentToken, Scanner.CurrentTokenLine, Scanner.CurrentTokenColumn));
            }
        }


        // Not in use, not supported
        // <suffixoperator> ::= "+" | "-"
        private ASTSuffixOperator ExpectSuffixOperator()
        {
            switch (this.Scanner.CurrentToken)
            {
                case TokenType.PLUS:
                    this.Expect(TokenType.PLUS);
                    return new ASTSuffixOperator()
                    {
                        Operator = AddOperatorType.ADD
                    };
                case TokenType.MINUS:
                    this.Expect(TokenType.MINUS);
                    return new ASTSuffixOperator()
                    {
                        Operator = AddOperatorType.SUBSTRACT
                    };
                default:
                    throw new Exception(String.Format("Expected {0} but got {1} at line {2} column {3}",
                        @"+ or -", Scanner.CurrentToken, Scanner.CurrentTokenLine, Scanner.CurrentTokenColumn));
            }
        }
    }
}
