using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler_v2.AST
{
    public interface IASTVisitor<T>
    {
        T Visit(ASTModel aSTModel);
        T Visit(ASTVerb aSTVerb);
        T Visit(ASTBinaryExpression aSTExpression);
        T Visit(ASTLiteral aSTLiteral);
        T Visit(ASTCall aSTCall);
        T Visit(ASTDirectSNA aSTDirectSNA);
        T Visit(ASTLValue aSTLValue);
    }
}
