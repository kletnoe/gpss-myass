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
        public MetadataRetriever_v2 MetadataRetriever { get; private set; }
        public List<string> ReferencedAssemblies { get; private set; }
        public ASTModel Model { get; private set; }

        public Parser_v2(IScanner scanner)
            : this(scanner, AssemblyCompiler.DefaultRefs.ToList())
        {

        }

        public Parser_v2(IScanner scanner, List<string> referencedAssemblies)
        {
            this.Scanner = scanner;
            this.ReferencedAssemblies = referencedAssemblies;

            this.Model = this.Parse();
        }

        private ASTModel Parse()
        {
            return this.ExpectModel();
        }

        private void ConstructMetadataRetriever(List<string> namespaces, List<string> types)
        {
            // Get default usings should be removed
            //namespaces.AddRange(AssemblyCompiler.DefaultNamespaces);
            //types.AddRange(AssemblyCompiler.DefaultTypes);
            //this.ReferencedAssemblies.AddRange(AssemblyCompiler.DefaultRefs);
            // 

            this.MetadataRetriever = new MetadataRetriever_v2(
                new HashSet<string>(this.ReferencedAssemblies),
                new HashSet<string>(namespaces),
                new HashSet<string>(types)
            );
        }

        // <leadingtrivia> ::= ( <WHITE> | <COMMENT> | <LF> )*
        private void ExpectLeadingTrivia()
        {
            while (this.Scanner.CurrentToken == TokenType.COMMENT
                || this.Scanner.CurrentToken == TokenType.WHITE
                || this.Scanner.CurrentToken == TokenType.LF)
            {
                switch (this.Scanner.CurrentToken)
                {
                    case TokenType.COMMENT:
                        this.Expect(TokenType.COMMENT);
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

        // <trailingtrivia> ::= [ <WHITE> ] [ <COMMENT>]
        private void ExpectTrailingTrivia()
        {
            if(this.Scanner.CurrentToken == TokenType.WHITE)
            {
                this.Expect(TokenType.WHITE);
            }

            if (this.Scanner.CurrentToken == TokenType.COMMENT)
            {
                this.Expect(TokenType.COMMENT);
            }
        }

        private string ExpectID()
        {
            string id = (string)this.Expect(TokenType.ID);
            return id;
        }

        private string ExpectQualID()
        {
            string id;

            switch (this.Scanner.CurrentToken)
            {
                case TokenType.QUALID:
                    id = (string)this.Expect(TokenType.QUALID);
                    break;
                case TokenType.ID:
                    id = (string)this.Expect(TokenType.ID);
                    break;
                default:
                    throw new ParserException(this, @"QUALID or ID");
            }

            return id;
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
            List<string> namespaces = new List<string>();
            List<string> types = new List<string>();

            while (this.Scanner.CurrentToken == TokenType.ATSIGN)
            {
                this.Expect(TokenType.ATSIGN);

                if (this.Scanner.CurrentToken == TokenType.USING)
                {
                    namespaces.Add(this.ExpectDirective());
                }
                else if (this.Scanner.CurrentToken == TokenType.USINGP)
                {
                    types.Add(this.ExpectDirective());
                }
                else
                {
                    throw new ParserException(this, @"USING or USINGP");
                }

                this.ExpectTrailingTrivia();
                this.Expect(TokenType.LF);
                this.ExpectLeadingTrivia();
            }

            // Construct metadata
            this.ConstructMetadataRetriever(namespaces, types);
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

        // <verb> ::= [ <ID> ( <WHITE> )+ ] <ID> [ ( <WHITE> )+ <operands> ]
        private ASTVerb ExpectVerb()
        {
            ASTVerb verb = new ASTVerb();

            string firstId = this.ExpectID();
            if (this.MetadataRetriever.IsVerb(firstId))
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

            if(this.Scanner.CurrentToken == TokenType.WHITE)
            {
                do
                {
                    this.Expect(TokenType.WHITE);
                } while (this.Scanner.CurrentToken == TokenType.WHITE);

                // Operands
                IList<IASTExpression> operands = this.ExpectOperands();
                foreach (var operand in operands)
                {
                    verb.Operands.Add(operand);
                }
            }

            //Console.WriteLine(verb);
            return verb;
        }

        // <operands> ::= <operand> ( ( <COMMA> | <WHITE> ) <operand> )*
        private IList<IASTExpression> ExpectOperands()
        {
            IList<IASTExpression> operands = new List<IASTExpression>();

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
                        throw new ParserException(this, @"WhiteSpace or Comma");
                }

                operands.Add(this.ExpectOperand());
            }

            return operands;
        }

        // <operand> ::= [ <expression> | <parexpression> ]
        private IASTExpression ExpectOperand()
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

        // <parexpression> ::= <LPAR> <expression> <RPAR>
        // Note that ParExpression turns scanners IgnoreWhitespace
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


        // <expression> ::= <term> ( <addop> <term> )*
        private IASTExpression ExpectExpression()
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

        // <term> ::= <signedfactor> ( <mulop> <factor> )*
        private IASTExpression ExpectTerm()
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
        private IASTExpression ExpectSignedFactor()
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

        // <factor> ::= <literal> | <call> | <LPAR> <expression> <RPAR>
        private IASTExpression ExpectFactor()
        {
            IASTExpression expression = null;

            switch (this.Scanner.CurrentToken)
            {
                case TokenType.NUMERIC:
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
                    throw new ParserException(this, @"ID or NUMERIC or '(' ");
            }

            return expression;
        }

        // <call> ::= <lval> | <procedurecall> | <snacall>
        //   <lval> ::= <ID>
        //   <snacall> ::= <ID> <DOLLAR> <ID>
        //   <procedurecall> ::= <ID> <LPAR> <actuals> <RPAR>
        private IASTCall ExpectCall()
        {
            string id = this.ExpectID();

            if(this.Scanner.CurrentToken == TokenType.LPAR)
            {
                ASTProcedureCall call = new ASTProcedureCall();
                call.ProcedureId = id;

                this.Expect(TokenType.LPAR);

                IList<IASTExpression> actuals = this.ExpectActuals();
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

        // <actuals> ::= [ <expression> ( <COMMA> <expression> )* ]
        private IList<IASTExpression> ExpectActuals()
        {
            IList<IASTExpression> actuals = new List<IASTExpression>();

            if (this.Scanner.CurrentToken == TokenType.ID
                || this.Scanner.CurrentToken == TokenType.NUMERIC
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

        // <literal> ::= <NUMBER>
        // <literal> ::= INT | DOUBLE | STRING
        private ASTLiteral ExpectLiteral()
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
