using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAssCompiler.AST;

namespace MyAssCompiler
{
    public class Parser : IParser
    {
        public IScanner Scanner { get; private set; }

        private Dictionary<int, string> idsList;

        public Dictionary<int, string> IdsList { get { return this.idsList; } }

        public Parser(IScanner scanner)
        {
            this.Scanner = scanner;
            this.idsList = new Dictionary<int, string>();
        }

        public ASTModel Parse()
        {
            return this.ExpectModel();
        }

        public int ExpectID()
        {
            int id = (int)this.Expect(TokenType.ID);
            if (!IdsList.ContainsKey(id))
            {
                IdsList.Add(id, this.Scanner.Identifiers.ElementAt(id));
            }

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


        // <model> ::= { <verb> }+
        public ASTModel ExpectModel()
        {
            ASTModel model = new ASTModel();

            while (this.Scanner.CurrentToken != TokenType.EOF)
            {
                // Eat comments and linefeeds;
                while (this.Scanner.CurrentToken == TokenType.COMMENT
                    || this.Scanner.CurrentToken == TokenType.LF)
                {
                    switch (this.Scanner.CurrentToken)
                    {
                        case TokenType.COMMENT:
                            this.Expect(TokenType.COMMENT);
                            break;
                        case TokenType.LF:
                            this.Expect(TokenType.LF);
                            break;
                    }
                }

                model.Verbs.Add(ExpectBlock());

                // Eat comment
                if (this.Scanner.CurrentToken == TokenType.COMMENT)
                {
                    this.Expect(TokenType.COMMENT);
                }

                this.Expect(TokenType.LF);
            }

            return model;
        }

        // <block> ::= [ ID ] ID [ <operator> ] [ <operands> ]
        public ASTBlock ExpectBlock()
        {
            ASTBlock block = new ASTBlock();

            int firstId;
            int? secondId = null;
            int? thirdId = null;
            int? fourthId = null;

            firstId = this.ExpectID();

            if (this.Scanner.CurrentToken == TokenType.ID)
            {
                secondId = this.ExpectID();
            }
            if (this.Scanner.CurrentToken == TokenType.ID)
            {
                thirdId = this.ExpectID();
            }

            if (this.Scanner.CurrentToken == TokenType.ID)
            {
                fourthId = this.ExpectID();
            }

            // id id id id 
            if (secondId.HasValue && thirdId.HasValue && fourthId.HasValue)
            {
                block.LabelId = firstId;
                block.VerbId = secondId.Value;
                block.Operator = this.ExpectOperator(thirdId.Value);
                block.Operands = this.ExpectOperands(fourthId.Value);
                block.IsResolved = true;
            }
            // id id id non-id
            else if (secondId.HasValue && thirdId.HasValue)
            {
                // There are two cases for this:
                // Label Block Operand1
                // Block Operator Operand1
                block.UnresolvedId1 = firstId;
                block.UnresolvedId2 = secondId.Value;
                block.Operands = this.ExpectOperands(thirdId.Value);
                block.IsResolved = false;
            }
            else if (secondId.HasValue)
            {
                if (this.Scanner.CurrentToken == TokenType.DOLLAR
                    || this.Scanner.CurrentToken == TokenType.LPAR)
                {
                    block.LabelId = null;
                    block.VerbId = firstId;
                    block.Operands = this.ExpectOperands(secondId.Value);
                    block.IsResolved = true;
                }
                else if (this.Scanner.CurrentToken == TokenType.NUMERIC)
                {
                    block.LabelId = firstId;
                    block.VerbId = secondId.Value;
                    block.Operands = this.ExpectOperands();
                    block.IsResolved = true;
                }
                else
                {
                    // There are two cases for this:
                    // Label Block
                    // Block Operand1
                    block.UnresolvedId1 = firstId;
                    block.UnresolvedId2 = secondId.Value;
                    block.Operands = this.ExpectOperands();
                    block.IsResolved = false;
                }
            }
            else
            {
                block.LabelId = null;
                block.VerbId = firstId;
                block.Operator = null;
                block.Operands = this.ExpectOperands();
                block.IsResolved = true;
            }


            Console.WriteLine(block);
            return block;
        }

        // <operator> ::= ID
        public ASTOperator ExpectOperator(int id)
        {
            return new ASTOperator()
            {
                Id = id
            };
        }

        // <operands> ::= <operand> { "," <operand> }
        public ASTOperands ExpectOperands(int? initialId = null)
        {
            ASTOperands operands = new ASTOperands();

            if (initialId.HasValue)
            {
                operands.Operands.Add(this.ExpectOperand(initialId.Value));
            }
            else
            {
                if (this.Scanner.CurrentToken == TokenType.ID
                    || this.Scanner.CurrentToken == TokenType.NUMERIC
                    || this.Scanner.CurrentToken == TokenType.LPAR)
                {
                    operands.Operands.Add(this.ExpectOperand());
                }
            }

            while (this.Scanner.CurrentToken == TokenType.COMMA)
            {
                this.Expect(TokenType.COMMA);
                operands.Operands.Add(this.ExpectOperand());
            }

            return operands;
        }

        // <operand> ::= "" | <expr> [ <suffixoperator> ]
        public ASTOperand ExpectOperand(int? initialId = null)
        {
            ASTOperand operand = null;

            if (initialId.HasValue)
            {
                operand = this.ExpectExpression(initialId);
            }
            else
            {
                if (this.Scanner.CurrentToken == TokenType.ID
                    || this.Scanner.CurrentToken == TokenType.NUMERIC
                    || this.Scanner.CurrentToken == TokenType.LPAR)
                {
                    operand = this.ExpectExpression();
                }
            }
            return operand;
        }

        // <expr> ::= <term> { <addop> <term> }
        public ASTExpression ExpectExpression(int? initialId = null)
        {
            ASTExpression expression = new ASTExpression();

            if (initialId.HasValue)
            {
                expression.LTerm = this.ExpectTerm(initialId);
            }
            else
            {
                expression.LTerm = this.ExpectTerm();
            }

            if (this.Scanner.CurrentToken == TokenType.PLUS
                || this.Scanner.CurrentToken == TokenType.MINUS)
            {
                expression.Operator = this.ExpectAddOperator();
                expression.RTerm = this.ExpectTerm();
            }

            return expression;
        }

        // <term> ::= <signedfactor> { <mulop> <factor> }
        public ASTTerm ExpectTerm(int? initialId = null)
        {
            ASTTerm term = new ASTTerm();

            if (initialId.HasValue)
            {
                term.LFactor = this.ExpectSignedFactor(initialId);
            }
            else
            {
                term.LFactor = this.ExpectSignedFactor();
            }

            if (this.Scanner.CurrentToken == TokenType.OCTOTROPE
                || this.Scanner.CurrentToken == TokenType.FWDSLASH
                || this.Scanner.CurrentToken == TokenType.BCKSLASH
                || this.Scanner.CurrentToken == TokenType.CARRET)
            {
                term.Operator = this.ExpectMulOperator();
                term.RFactor = this.ExpectFactor();
            }

            return term;
        }

        // <signedfactor> ::= [ <addop> ] <factor>
        public ASTSignedFactor ExpectSignedFactor(int? initialId = null)
        {
            ASTSignedFactor factor = new ASTSignedFactor();

            if (initialId.HasValue)
            {
                factor.Value = this.ExpectFactor(initialId);
            }
            else
            {
                if (this.Scanner.CurrentToken == TokenType.PLUS
                    || this.Scanner.CurrentToken == TokenType.MINUS)
                {
                    factor.Operator = this.ExpectAddOperator();
                }
                factor.Value = ExpectFactor();
            }

            return factor;
        }

        // <factor> ::= <literal> | <lval> | "(" <expression> ")"
        public IASTFactor ExpectFactor(int? initialId = null)
        {
            IASTFactor factor = null;

            if (initialId.HasValue)
            {
                factor = this.ExpectLValue(initialId);
            }
            else
            {
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
                }
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
        public ASTLValue ExpectLValue(int? initialId = null)
        {
            ASTLValue lvalue = new ASTLValue();

            if (initialId.HasValue)
            {
                lvalue.Id = initialId.Value;
            }
            else
            {
                lvalue.Id = this.ExpectID();
            }

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
                case TokenType.OCTOTROPE:
                    this.Expect(TokenType.OCTOTROPE);
                    return MulOperatorType.MULTIPLY;
                case TokenType.FWDSLASH:
                    this.Expect(TokenType.FWDSLASH);
                    return MulOperatorType.DIVIDE;
                case TokenType.BCKSLASH:
                    this.Expect(TokenType.BCKSLASH);
                    return MulOperatorType.MODULO;
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
