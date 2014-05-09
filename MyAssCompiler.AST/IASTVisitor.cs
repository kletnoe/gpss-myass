using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public interface IASTVisitor
    {
        void Visit(ASTActuals node);
        void Visit(ASTBinaryExpression node);
        void Visit(ASTBlock node);
        void Visit(ASTOperator node);
        void Visit(ASTCall node);
        void Visit(ASTDirectSNA node);
        void Visit(ASTLiteral node);
        void Visit(ASTLValue node);
        void Visit(ASTModel node);
        void Visit(ASTOperands node);
        void Visit(ASTPostfixUnaryExpression node);
        void Visit(ASTPrefixUnaryExpression node);
    }
}
