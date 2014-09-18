using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Compiler
{
    public enum TokenType
    {
        NUMERIC,
        ID,
        QUALID,
        COMMENT,
        WHITE,

        LF,

        // BinaryOperator
        PLUS,               // '+'
        MINUS,              // '-'
        OCTOTHORPE,         // '#'
        FWDSLASH,           // '/'
        BCKSLASH,           // '\'
        CARRET,             // '^'

        LPAR,       // '('
        RPAR,       // ')'
        DOLLAR,     // '$'
        ASTERISK,   // '*'
        COMMA,      // ','
        ATSIGN,     // '@'

        ILLEGAL,
        EOF,        // '\0'

        // Keywords
        USING,
        USINGP,
    }
}
