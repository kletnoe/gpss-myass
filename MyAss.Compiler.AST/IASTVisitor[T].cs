using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssCompiler.AST
{
    public interface IASTVisitor<T>
    {
        T Visit(ASTModel node);
        T Visit(ASTVerb node);
        T Visit(ASTOperands node);
        T Visit(ASTExpression node);
        T Visit(ASTAdditive node);
        T Visit(ASTTerm node);
        T Visit(ASTMultiplicative node);

        T Visit(ASTActuals node);
        T Visit(ASTSignedFactor node);
        T Visit(ASTCall node);
        T Visit(ASTDirectSNA node);
        T Visit(ASTSuffixOperator node);
        T Visit(ASTLiteral node);
        T Visit(ASTLValue node);
    }
}
