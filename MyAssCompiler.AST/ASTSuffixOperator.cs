﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public class ASTSuffixOperator : IASTNode
    {
        public AddOperatorType Operator { get; set; }

        public void Accept(IASTVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public T Accept<T>(IASTVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return this.Operator.ToString();
        }
    }
}
