using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Compiler
{
    public class Scanner : IScanner
    {
        public ICharSource CharSource { get; private set; }

        private TokenType currentToken;
        private object currentTokenVal;
        private int currentTokenLine;
        private int currentTokenColumn;
        public bool ignoreWhitespace;

        public TokenType CurrentToken { get { return this.currentToken; } }
        public object CurrentTokenVal { get { return this.currentTokenVal; } }
        public int CurrentTokenLine { get { return this.currentTokenLine; } }
        public int CurrentTokenColumn { get { return this.currentTokenColumn; } }
        public bool IgnoreWhitespace { get { return this.ignoreWhitespace; } set { this.ignoreWhitespace = value; } }


        public Scanner(ICharSource charSource)
        {
            this.CharSource = charSource;
            this.Next();
        }

        private void Ret(TokenType token, object value)
        {
            this.currentToken = token;
            this.currentTokenVal = value;

            //Console.WriteLine(token + " \t" + value);
        }

        private void Ret(TokenType token)
        {
            this.Ret(token, -1);
            this.CharSource.Next();
        }

        public void Next()
        {
            if (this.IgnoreWhitespace)
            {
                while (Char.IsWhiteSpace(this.CharSource.CurrentChar) && this.CharSource.CurrentChar != '\n')
                {
                    this.CharSource.Next();
                }
            }
            this.currentTokenLine = this.CharSource.Line;
            this.currentTokenColumn = this.CharSource.Column;

            switch (this.CharSource.CurrentChar)
            {
                case '\0':
                    this.Ret(TokenType.EOF);
                    break;

                case '+':
                    this.Ret(TokenType.PLUS);
                    break;
                case '-':
                    this.Ret(TokenType.MINUS);
                    break;
                case '#':
                    this.Ret(TokenType.OCTOTHORPE);
                    break;
                case '/':
                    this.Ret(TokenType.FWDSLASH);
                    break;
                case '\\':
                    this.Ret(TokenType.BCKSLASH);
                    break;
                case '^':
                    this.Ret(TokenType.CARRET);
                    break;

                case '(':
                    this.Ret(TokenType.LPAR);
                    break;
                case ')':
                    this.Ret(TokenType.RPAR);
                    break;
                case '$':
                    this.Ret(TokenType.DOLLAR);
                    break;
                case '*':
                    this.Ret(TokenType.ASTERISK);
                    break;
                case ',':
                    this.Ret(TokenType.COMMA);
                    break;

                case '\r':
                case '\n':
                    this.Ret(TokenType.LF);
                    break;

                default:
                    if (char.IsLetter(this.CharSource.CurrentChar))
                    {
                        this.RetIdOrKwd();
                    }
                    else if (Char.IsDigit(this.CharSource.CurrentChar) || this.CharSource.CurrentChar == '.')
                    {
                        this.RetNumber();
                    }
                    else if (this.CharSource.CurrentChar == ';')
                    {
                        this.RetComment();
                    }
                    else if (Char.IsWhiteSpace(this.CharSource.CurrentChar) && this.CharSource.CurrentChar != '\n')
                    {
                        this.Ret(TokenType.WHITE);
                    }
                    else
                    {
                        this.Ret(TokenType.ILLEGAL);
                    }
                    break;
            }
        }

        private void RetComment()
        {
            string buffer = "";
            do
            {
                buffer += this.CharSource.CurrentChar;
                this.CharSource.Next();
            } while (this.CharSource.CurrentChar != '\n' && this.CharSource.CurrentChar != '\0');

            this.Ret(TokenType.COMMENT, buffer);
        }

        private void RetIdOrKwd()
        {
            string buffer = "";
            do
            {
                buffer += this.CharSource.CurrentChar;
                this.CharSource.Next();
            } while (char.IsLetterOrDigit(this.CharSource.CurrentChar));

            this.Ret(TokenType.ID, buffer);
        }

        private void RetNumber()
        {
            string buffer = "";
            do
            {
                buffer += this.CharSource.CurrentChar;
                this.CharSource.Next();
            } while (Char.IsDigit(this.CharSource.CurrentChar) || this.CharSource.CurrentChar == '.');

            try
            {
                if (buffer.Contains("."))
                {
                    this.Ret(TokenType.NUMERIC, Double.Parse(buffer));
                }
                else
                {
                    this.Ret(TokenType.NUMERIC, Int32.Parse(buffer));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Tokenizing error occurred @ line {0} column {1}. Error message: {2}",
                    this.CharSource.Line, this.CharSource.Column, e.Message));
            }
        }
    }
}
