using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAss.Compiler.AST
{
    public interface IASTVisitor
    {
        void Visit(ASTModel node);
        void Visit(ASTVerb node);
        void Visit(ASTOperands node);
        void Visit(ASTExpression node);
        void Visit(ASTAdditive node);
        void Visit(ASTTerm node);
        void Visit(ASTMultiplicative node);

        void Visit(ASTActuals node);
        void Visit(ASTSignedFactor node);
        void Visit(ASTCall node);
        void Visit(ASTDirectSNA node);
        void Visit(ASTSuffixOperator node);
        void Visit(ASTLiteral node);
        void Visit(ASTLValue node);
    }
}
