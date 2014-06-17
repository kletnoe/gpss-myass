using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.CodeGeneration
{
    public class CompilerException : Exception
    {
        public CompilerException()
        {
        }

        public CompilerException(string message)
        : base(message)
        {
        }

        public CompilerException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
