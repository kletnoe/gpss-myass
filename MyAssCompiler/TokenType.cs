using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler
{
    public enum TokenType
    {
        NUMERIC,
        ID,
        COMMENT,

        LF,

        // BinaryOperator
        PLUS,               // '+'
        MINUS,              // '-'
        OCTOTHORPE,          // '#'
        FWDSLASH,           // '/'
        BCKSLASH,           // '\'
        CARRET,             // '^'

        LPAR,       // '('
        RPAR,       // ')'
        DOLLAR,     // '$'
        ASTERISK,   // '*'
        COMMA,      // ','

        ILLEGAL,
        EOF,        // '\0'
    }
}
