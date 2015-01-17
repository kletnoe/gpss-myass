using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler_v2.AST;
using MyAss.Compiler.Metadata;
using MyAss.Compiler;

namespace MyAss.Compiler
{
    public class Parser
    {
        public IScanner Scanner { get; private set; }
        public MetadataRetriever MetadataRetriever { get; private set; }
        public List<string> ReferencedAssemblies { get; private set; }
        public List<string> UsedNamespaces { get; private set; }
        public List<string> UsedTypes { get; private set; }

        public ASTModel Model { get; private set; }

        public Parser(IScanner scanner)
            : this(scanner, AssemblyCompiler.DefaultRefs.ToList())
        {

        }

        public Parser(IScanner scanner, List<string> referencedAssemblies)
        {
            this.Scanner = scanner;
            this.ReferencedAssemblies = referencedAssemblies;
            this.MetadataRetriever = new MetadataRetriever(referencedAssemblies);
            this.UsedNamespaces = new List<string>();
            this.UsedTypes = new List<string>();

            this.Model = this.Parse();
        }

        private ASTModel Parse()
        {
            return this.ExpectModel();
        }

        private bool IsVerbId(string id)
        {
            if (id.Contains('.'))
            {
                return this.MetadataRetriever.IsVerbName(id);
            }
            else
            {
                return this.MetadataRetriever.IsVerbName(this.UsedNamespaces, id);
            }
        }

        private string ResolveVerbId(string id)
        {
            if (id.Contains('.'))
            {
                return this.MetadataRetriever.ResolveVerbName(id);
            }
            else
            {
                return this.MetadataRetriever.ResolveVerbName(this.UsedNamespaces, id);
            }
        }

        private string ResolveFunctionId(string id)
        {
            if (id.Contains('.'))
            {
                return this.MetadataRetriever.ResolveFunctionName(id);
            }
            else
            {
                return this.MetadataRetriever.ResolveFunctionName(this.UsedTypes, id);
            }
        }

        // Single token
        private object Expect(TokenType expected)
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
                throw new ParserException(this, expected.ToString());
            }
        }

        // <leadingtrivia> ::= ( <WHITE> | <comment> | <LF> )*
        private void ExpectLeadingTrivia()
        {
            while (this.Scanner.CurrentToken == TokenType.WHITE
                || this.Scanner.CurrentToken == TokenType.SEMICOL
                || this.Scanner.CurrentToken == TokenType.LF)
            {
                switch (this.Scanner.CurrentToken)
                {
                    case TokenType.SEMICOL:
                        this.ExpectComment();
                        break;
                    case TokenType.WHITE:
                        this.Expect(TokenType.WHITE);
                        break;
                    case TokenType.LF:
                        this.Expect(TokenType.LF);
                        break;
                }
            }
        }

        // <trailingtrivia> ::= [ <WHITE> ] [ <comment>]
        private void ExpectTrailingTrivia()
        {
            if(this.Scanner.CurrentToken == TokenType.WHITE)
            {
                this.Expect(TokenType.WHITE);
            }

            if (this.Scanner.CurrentToken == TokenType.SEMICOL)
            {
                this.ExpectComment();
            }
        }

        // <comment> ::= <SEMICOL> ( <any-char> )*
        private void ExpectComment()
        {
            this.Expect(TokenType.SEMICOL);

            while (this.Scanner.CurrentToken != TokenType.LF
                && this.Scanner.CurrentToken != TokenType.EOF)
            {
                this.ExpectAnyToken();
            }
        }

        // <any-token> ::= any token except <LF> and <EOF>
        private void ExpectAnyToken()
        {
            if (this.Scanner.CurrentToken != TokenType.LF
                && this.Scanner.CurrentToken != TokenType.EOF)
            {
                this.Expect(this.Scanner.CurrentToken);
            }
        }

        // <id>
        private string ExpectID()
        {
            string id = (string)this.Expect(TokenType.ID);
            return id;
        }

        // <qual-id> ::= (<id> <PERIOD> )* <id>
        // TODO: Temporary returns only last id
        private string ExpectQualID()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.ExpectID());

            while (this.Scanner.CurrentToken == TokenType.PERIOD)
            {
                this.Expect(TokenType.PERIOD);
                sb.Append('.');
                sb.Append(this.ExpectID());
            }

            return sb.ToString();
        }

        // <number> ::= <integer> [ <PERIOD> <integer> ]
        private double ExpectNumber()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((string)this.Expect(TokenType.INTEGER));

            if (this.Scanner.CurrentToken == TokenType.PERIOD)
            {
                this.Expect(TokenType.PERIOD);
                sb.Append('.');
                sb.Append((string)this.Expect(TokenType.INTEGER));
            }

            double result = Double.Parse(sb.ToString());
            return result;
        }


        // <model> ::= <leadingtrivia> <directives> <verbs> <EOF>
        private ASTModel ExpectModel()
        {
            ASTModel model = new ASTModel();

            this.ExpectLeadingTrivia();
                
            this.ExpectDirectives();

            IList<ASTVerb> verbs = this.ExpectVerbs();
            foreach (var verb in verbs)
            {
                model.Verbs.Add(verb);
            }

            this.Expect(TokenType.EOF);

            return model;
        }

        // <directives> :: = ( <ATSIGN> <directive> <trailingtrivia> <LF> <leadingtrivia> )*
        private void ExpectDirectives()
        {
            while (this.Scanner.CurrentToken == TokenType.ATSIGN)
            {
                this.Expect(TokenType.ATSIGN);

                if (this.Scanner.CurrentToken == TokenType.USING)
                {
                    this.UsedNamespaces.Add(this.ExpectDirective());
                }
                else if (this.Scanner.CurrentToken == TokenType.USINGP)
                {
                    this.UsedTypes.Add(this.ExpectDirective());
                }
                else
                {
                    throw new ParserException(this, @"USING or USINGP");
                }

                this.ExpectTrailingTrivia();
                this.Expect(TokenType.LF);
                this.ExpectLeadingTrivia();
            }
        }

        // <directive> ::= <USING> | <USINGP> ( <WHITE> )+ <QUAL-ID>
        private string ExpectDirective()
        {
            if (this.Scanner.CurrentToken == TokenType.USING)
            {
                this.Expect(TokenType.USING);
            }
            else if (this.Scanner.CurrentToken == TokenType.USINGP)
            {
                this.Expect(TokenType.USINGP);
            }
            else
            {
                throw new ParserException(this, @"USING or USINGP");
            }

            do
            {
                this.Expect(TokenType.WHITE);
            } while (this.Scanner.CurrentToken == TokenType.WHITE);

            return this.ExpectQualID();
        }

        // <verbs> ::= ( <verb> <trailingtrivia> <LF> <leadingtrivia> )*
        private IList<ASTVerb> ExpectVerbs()
        {
            IList<ASTVerb> verbs = new List<ASTVerb>();

            while(this.Scanner.CurrentToken == TokenType.ID)
            {
                verbs.Add(ExpectVerb());

                this.ExpectTrailingTrivia();
                this.Expect(TokenType.LF);
                this.ExpectLeadingTrivia();
            }

            return verbs;
        }

        // <verb> ::= [ <id> ( <WHITE> )+ ] <qual-id> [ ( <WHITE> )+ <operands> ]
        // TODO: Temporary hack, <qual-id> for label 
        private ASTVerb ExpectVerb()
        {
            ASTVerb verb = new ASTVerb();

            string currentId = this.ExpectQualID();

            if (!this.IsVerbId(currentId))
            {
                // Verb has Label
                verb.LabelId = currentId;

                do
                {
                    this.Expect(TokenType.WHITE);
                } while (this.Scanner.CurrentToken == TokenType.WHITE);

                currentId = this.ExpectQualID();
            }

            verb.VerbId = this.ResolveVerbId(currentId);

            if(this.Scanner.CurrentToken == TokenType.WHITE)
            {
                do
                {
                    this.Expect(TokenType.WHITE);
                } while (this.Scanner.CurrentToken == TokenType.WHITE);

                // Operands
                IList<ASTAnyExpression> operands = this.ExpectOperands();
                foreach (var operand in operands)
                {
                    verb.Operands.Add(operand);
                }
            }

            //// Operands
            //if (this.Scanner.CurrentToken == TokenType.LPAR
            //    || this.Scanner.CurrentToken == TokenType.ID
            //    || this.Scanner.CurrentToken == TokenType.INTEGER
            //    || this.Scanner.CurrentToken == TokenType.MINUS
            //    || this.Scanner.CurrentToken == TokenType.PLUS)
            //{
            //    IList<ASTAnyExpression> operands = this.ExpectOperands();
            //    foreach (var operand in operands)
            //    {
            //        verb.Operands.Add(operand);
            //    }
            //}

            //Console.WriteLine(verb);
            return verb;
        }

        // <operands> ::= <operand> ( ( <COMMA> | <WHITE> ) <operand> )*
        private IList<ASTAnyExpression> ExpectOperands()
        {
            IList<ASTAnyExpression> operands = new List<ASTAnyExpression>();

            operands.Add(this.ExpectOperand());

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
                        throw new ParserException(this, @"WHITE or COMMA");
                }

                operands.Add(this.ExpectOperand());
            }

            // Hack: Remove trailing null operands.
            operands = operands.TakeWhile((x, index) => operands.Skip(index).Any(w => w != null)).ToList();

            return operands;
        }

        // <operand> ::= [ <expression> | <parexpression> ]
        private ASTAnyExpression ExpectOperand()
        {
            ASTAnyExpression operand = null;

            if (this.Scanner.CurrentToken == TokenType.LPAR)
            {
                operand = this.ExpectParExpression();
            }
            else if (this.Scanner.CurrentToken == TokenType.ID
                || this.Scanner.CurrentToken == TokenType.INTEGER
                || this.Scanner.CurrentToken == TokenType.MINUS
                || this.Scanner.CurrentToken == TokenType.PLUS)
            {
                operand = this.ExpectExpression();
            }

            return operand;
        }

        // <parexpression> ::= <LPAR> <expression> <RPAR>
        // Note that ParExpression turns scanners IgnoreWhitespace
        private ASTAnyExpression ExpectParExpression()
        {
            ASTAnyExpression operand = null;

            this.Scanner.IgnoreWhitespace = true;
            this.Expect(TokenType.LPAR);

            operand = ExpectExpression();

            this.Scanner.IgnoreWhitespace = false;
            this.Expect(TokenType.RPAR);

            return operand;
        }


        // <expression> ::= <term> ( <addop> <term> )*
        private ASTAnyExpression ExpectExpression()
        {
            ASTAnyExpression expression = this.ExpectTerm();

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

        // <term> ::= <signedfactor> ( <mulop> <factor> )*
        private ASTAnyExpression ExpectTerm()
        {
            ASTAnyExpression expression = this.ExpectSignedFactor();

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
        private ASTAnyExpression ExpectSignedFactor()
        {
            ASTAnyExpression expression;

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

        // <factor> ::= <literal> | <call> | <LPAR> <expression> <RPAR>
        private ASTAnyExpression ExpectFactor()
        {
            ASTAnyExpression expression = null;

            switch (this.Scanner.CurrentToken)
            {
                case TokenType.INTEGER:
                    expression = this.ExpectLiteral();
                    break;
                case TokenType.ID:
                    expression = this.ExpectCall();
                    break;
                case TokenType.LPAR:
                    this.Expect(TokenType.LPAR);
                    expression = ExpectExpression();
                    this.Expect(TokenType.RPAR);
                    break;
                default:
                    throw new ParserException(this, @"ID or INTEGER or LPAR ");
            }

            return expression;
        }

        // <call> ::= <lval> | <procedurecall> | <snacall>
        //   <lval> ::= <ID>
        //   <snacall> ::= <ID> <DOLLAR> <ID>
        //   <procedurecall> ::= <ID> <LPAR> <actuals> <RPAR>
        private ASTAnyCall ExpectCall()
        {
            string id = this.ExpectQualID();

            if(this.Scanner.CurrentToken == TokenType.LPAR)
            {
                ASTProcedureCall call = new ASTProcedureCall();
                call.ProcedureId = this.ResolveFunctionId(id);

                this.Expect(TokenType.LPAR);

                IList<ASTAnyExpression> actuals = this.ExpectActuals();
                foreach (var actual in actuals)
                {
                    call.Actuals.Add(actual);
                }

                this.Expect(TokenType.RPAR);

                return call;
            }
            else if(this.Scanner.CurrentToken == TokenType.DOLLAR)
            {
                this.Expect(TokenType.DOLLAR);
                return new ASTDirectSNACall()
                {
                    SnaId = this.ResolveFunctionId(id),
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

        // <actuals> ::= [ <expression> ( <COMMA> <expression> )* ]
        private IList<ASTAnyExpression> ExpectActuals()
        {
            IList<ASTAnyExpression> actuals = new List<ASTAnyExpression>();

            if (this.Scanner.CurrentToken == TokenType.ID
                || this.Scanner.CurrentToken == TokenType.INTEGER
                || this.Scanner.CurrentToken == TokenType.MINUS
                || this.Scanner.CurrentToken == TokenType.PLUS
                || this.Scanner.CurrentToken == TokenType.LPAR)
            {
                actuals.Add(this.ExpectExpression());

                while (this.Scanner.CurrentToken == TokenType.COMMA)
                {
                    this.Expect(TokenType.COMMA);
                    actuals.Add(this.ExpectExpression());
                }
            }

            return actuals;
        }

        private ASTLiteral ExpectLiteral()
        {
            double value = this.ExpectNumber();

            return new ASTLiteral()
            {
                LiteralType = LiteralType.Double,
                Value = value
            };
        }

        //// <literal> ::= <NUMBER>
        //// <literal> ::= INT | DOUBLE | STRING
        //private ASTLiteral ExpectLiteral()
        //{
        //    object value = this.Expect(TokenType.NUMERIC);
        //    LiteralType type;

        //    if (value is Int32)
        //    {
        //        type = LiteralType.Int32;
        //    }
        //    else if (value is Double)
        //    {
        //        type = LiteralType.Double;
        //    }
        //    else
        //    {
        //        type = LiteralType.String;
        //    }

        //    return new ASTLiteral()
        //    {
        //        LiteralType = type,
        //        Value = value
        //    };
        //}

        // <addop> ::= <PLUS> | <MINUS>
        private BinaryOperatorType ExpectAddOperator()
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
                    throw new ParserException(this, @"+ or -");
            }
        }

        // <mulop> ::= <OCTOTHORPE> | <FWDSLASH> | <BCKSLASH> | <CARRET>
        private BinaryOperatorType ExpectMulOperator()
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
                    throw new ParserException(this, @"# or / or \ or ^");
            }
        }
    }
}
