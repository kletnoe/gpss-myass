using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler_v2.AST;
using MyAss.Compiler.Metadata;
using MyAss.Compiler;

namespace MyAss.Compiler_v2
{
    public class Parser_v2
    {
        public IScanner Scanner { get; private set; }

        public Parser_v2(IScanner scanner)
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

        // <verb> ::= [ ID ] ID { <operand> }
        public ASTVerb ExpectVerb()
        {
            ASTVerb verb = new ASTVerb();

            string firstId = this.ExpectID();
            if (MetadataRetriever.IsBuiltinVerb(firstId))
            {
                // Id is verb 
                verb.VerbId = firstId;
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
            }

            do
            {
                this.Expect(TokenType.WHITE);
            } while (this.Scanner.CurrentToken == TokenType.WHITE);

            // Operands

            verb.Operands.Add(this.ExpectOperand());

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

                verb.Operands.Add(this.ExpectOperand());
            }


            Console.WriteLine(verb);
            return verb;
        }

        // <operand> ::= "" | <expression> | <parexpression>
        public IASTExpression ExpectOperand()
        {
            IASTExpression operand = null;

            if (this.Scanner.CurrentToken == TokenType.LPAR)
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

        private IASTExpression ExpectParExpression()
        {
            IASTExpression operand = null;

            this.Scanner.IgnoreWhitespace = true;
            this.Expect(TokenType.LPAR);

            operand = ExpectExpression();

            this.Scanner.IgnoreWhitespace = false;
            this.Expect(TokenType.RPAR);

            return operand;
        }


        // <expression> ::= <term> { <addop> <term> }
        public IASTExpression ExpectExpression()
        {
            IASTExpression expression = this.ExpectTerm();

            while (this.Scanner.CurrentToken == TokenType.PLUS
                || this.Scanner.CurrentToken == TokenType.MINUS)
            {
                ASTBinaryExpression binary = new ASTBinaryExpression();
                binary.Left = expression;
                binary.Operator = this.ExpectAddOperator();
                binary.Right = this.ExpectTerm();

                expression = binary;
            }

            return expression;
        }

        // <term> ::= ( <factor> | <signedfactor> ) { <mulop> <factor> }
        public IASTExpression ExpectTerm()
        {
            IASTExpression expression = this.ExpectSignedFactor();

            while (this.Scanner.CurrentToken == TokenType.OCTOTHORPE
                || this.Scanner.CurrentToken == TokenType.FWDSLASH
                || this.Scanner.CurrentToken == TokenType.BCKSLASH
                || this.Scanner.CurrentToken == TokenType.CARRET)
            {
                ASTBinaryExpression binary = new ASTBinaryExpression();
                binary.Left = expression;
                binary.Operator = this.ExpectMulOperator();
                binary.Right = this.ExpectFactor();

                expression = binary;
            }

            return expression;
        }

        // <signedfactor> ::= [ <addop> ] <factor>
        public IASTExpression ExpectSignedFactor()
        {
            IASTExpression expression;

            if (this.Scanner.CurrentToken == TokenType.PLUS
                || this.Scanner.CurrentToken == TokenType.MINUS)
            {
                ASTBinaryExpression binary = new ASTBinaryExpression();
                binary.Left = new ASTLiteral()
                {
                    LiteralType = LiteralType.Int32,
                    Value = 0
                };
                binary.Operator = this.ExpectAddOperator();
                binary.Right = ExpectFactor();

                expression = binary;
            }
            else
            {
                expression = ExpectFactor();
            }

            return expression;
        }

        // <factor> ::= <literal> | <lval> | "(" <expression> ")"
        public IASTExpression ExpectFactor()
        {
            IASTExpression expression = null;

            switch (this.Scanner.CurrentToken)
            {
                case TokenType.ID:
                    expression = this.ExpectCall();
                    break;
                case TokenType.NUMERIC:
                    expression = this.ExpectLiteral();
                    break;
                case TokenType.LPAR:
                    this.Expect(TokenType.LPAR);
                    expression = ExpectExpression();
                    this.Expect(TokenType.RPAR);
                    break;
                default:
                    throw new Exception(String.Format("Expected {0} but got {1} at line {2} column {3}",
                        @"ID or NUMERIC or '(' ", Scanner.CurrentToken, Scanner.CurrentTokenLine, Scanner.CurrentTokenColumn));
            }

            return expression;
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
        // <accessor> ::= <call> | <directsna>
        // <call> ::= "(" ( "" | <expr> { "," <expr> } ) ")"
        // <directsna> ::= "$" ID
        public IASTCall ExpectCall()
        {
            string id = this.ExpectID();

            if(this.Scanner.CurrentToken == TokenType.LPAR)
            {
                ASTProcedureCall call = new ASTProcedureCall();
                call.ProcedureId = id;

                this.Expect(TokenType.LPAR);

                if (this.Scanner.CurrentToken == TokenType.ID
                    || this.Scanner.CurrentToken == TokenType.NUMERIC)
                {
                    call.Actuals.Add(this.ExpectExpression());

                    while (this.Scanner.CurrentToken == TokenType.COMMA)
                    {
                        this.Expect(TokenType.COMMA);
                        call.Actuals.Add(this.ExpectExpression());
                    }
                }

                this.Expect(TokenType.RPAR);

                return call;
            }
            else if(this.Scanner.CurrentToken == TokenType.DOLLAR)
            {
                this.Expect(TokenType.DOLLAR);
                return new ASTDirectSNACall()
                {
                    SnaId = id,
                    ActualId = ExpectID()
                };
            }
            else
            {
                return new ASTLValue()
                {
                    Id = id
                };
            }
        }

        //// <lval> ::= ID [ <accessor> ]
        //public ASTLValue ExpectLValue()
        //{
        //    ASTLValue lvalue = new ASTLValue();

        //    lvalue.Id = this.ExpectID();

        //    if (this.Scanner.CurrentToken == TokenType.LPAR
        //        || this.Scanner.CurrentToken == TokenType.DOLLAR)
        //    {
        //        lvalue.Accessor = this.ExpectAccessor();
        //    }

        //    return lvalue;
        //}

        //// <accessor> ::= <call> | <directsna>
        //public IASTAccessor ExpectAccessor()
        //{
        //    switch (this.Scanner.CurrentToken)
        //    {
        //        case TokenType.LPAR:
        //            return this.ExpectCall();
        //        case TokenType.DOLLAR:
        //            return this.ExpectDirectSNA();
        //        default:
        //            throw new Exception(String.Format("Expected {0} but got {1} at line {2} column {3}",
        //                @"( or $ or + or -", Scanner.CurrentToken, Scanner.CurrentTokenLine, Scanner.CurrentTokenColumn));
        //    }
        //}

        //// <call> ::= "(" ( "" | <expr> { "," <expr> } ) ")"
        //public ASTProcedureCall ExpectCall_old()
        //{
        //    ASTProcedureCall call = new ASTProcedureCall();

        //    this.Expect(TokenType.LPAR);

        //    if (this.Scanner.CurrentToken == TokenType.ID
        //        || this.Scanner.CurrentToken == TokenType.NUMERIC)
        //    {
        //        call.Actuals.Add(this.ExpectExpression());

        //        while (this.Scanner.CurrentToken == TokenType.COMMA)
        //        {
        //            this.Expect(TokenType.COMMA);
        //            call.Actuals.Add(this.ExpectExpression());
        //        }
        //    }

        //    this.Expect(TokenType.RPAR);

        //    return call;
        //}

        //// <directsna> ::= "$" ID
        //public ASTDirectSNACall ExpectDirectSNA()
        //{
        //    this.Expect(TokenType.DOLLAR);
        //    return new ASTDirectSNACall()
        //    {
        //        ActualId = ExpectID()
        //    };
        //}

        // <addop> ::= "+" | "-"
        public BinaryOperatorType ExpectAddOperator()
        {
            switch (this.Scanner.CurrentToken)
            {
                case TokenType.PLUS:
                    this.Expect(TokenType.PLUS);
                    return BinaryOperatorType.Add;
                case TokenType.MINUS:
                    this.Expect(TokenType.MINUS);
                    return BinaryOperatorType.Substract;
                default:
                    throw new Exception(String.Format("Expected {0} but got {1} at line {2} column {3}",
                        @"+ or -", Scanner.CurrentToken, Scanner.CurrentTokenLine, Scanner.CurrentTokenColumn));
            }
        }

        // <mulop> ::= "#" | "/" | "%" | "^"
        public BinaryOperatorType ExpectMulOperator()
        {
            switch (this.Scanner.CurrentToken)
            {
                case TokenType.OCTOTHORPE:
                    this.Expect(TokenType.OCTOTHORPE);
                    return BinaryOperatorType.Multiply;
                case TokenType.FWDSLASH:
                    this.Expect(TokenType.FWDSLASH);
                    return BinaryOperatorType.Divide;
                case TokenType.BCKSLASH:
                    this.Expect(TokenType.BCKSLASH);
                    return BinaryOperatorType.Modulus;
                case TokenType.CARRET:
                    this.Expect(TokenType.CARRET);
                    return BinaryOperatorType.Exponent;
                default:
                    throw new Exception(String.Format("Expected {0} but got {1} at line {2} column {3}",
                        @"# or / or \ or ^", Scanner.CurrentToken, Scanner.CurrentTokenLine, Scanner.CurrentTokenColumn));
            }
        }
    }
}
