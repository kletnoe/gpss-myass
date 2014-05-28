﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public interface IASTVisitor
    {
        void Visit(ASTActuals node);
        void Visit(ASTExpression node);
        void Visit(ASTTerm node);
        void Visit(ASTSignedFactor node);
        void Visit(ASTBlock node);
        void Visit(ASTOperator node);
        void Visit(ASTCall node);
        void Visit(ASTDirectSNA node);
        void Visit(ASTSuffixOperator node);
        void Visit(ASTLiteral node);
        void Visit(ASTLValue node);
        void Visit(ASTModel node);
        void Visit(ASTOperands node);
    }
}