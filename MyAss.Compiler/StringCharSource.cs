using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Compiler
{
    public class StringCharSource : ICharSource
    {
        private string source;

        private int position;
        private int line;
        private int column;
        private char currentChar;
        private string currentLine;

        public int Position { get { return this.position; } }
        public int Line { get { return this.line; } }
        public int Column { get { return this.column; } }
        public char CurrentChar { get { return this.currentChar; } }

        public string CurrentLine
        {
            get
            {
                return this.source.Split('\n')[this.line - 1];
            }
        }

        public StringCharSource(string source)
        {
            this.position = 0;
            this.line = 1;
            this.column = 1;

            this.source = source + "\r\n"+ "\0\0\0"; // Three zigas
            this.currentChar = this.source[this.position];
        }

        public void Next()
        {
            this.position++;
            switch (this.currentChar)
            {
                case '\0':
                    break;
                case '\n':
                    this.line++;
                    this.column = 1;
                    this.currentChar = this.source[this.position];
                    break;
                default:
                    this.column++;
                    this.currentChar = this.source[this.position];
                    break;
            }
        }
    }
}
