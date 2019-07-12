using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Compiler.AST
{
    public interface IASTVisitor<T>
    {
        T Visit(ASTModel aSTModel);
        T Visit(ASTVerb aSTVerb);
        T Visit(ASTBinaryExpression aSTExpression);
        T Visit(ASTLiteral aSTLiteral);
        T Visit(ASTProcedureCall aSTCall);
        T Visit(ASTDirectSNACall aSTDirectSNA);
        T Visit(ASTLValue aSTLValue);
    }
}
