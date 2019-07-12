using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyAss.Compiler.Metadata;
using MyAss.Compiler.AST;
using MyAss.Framework;
using MyAss.Framework.Blocks;
using MyAss.Framework.Commands;
using MyAss.Framework.OperandTypes;

namespace MyAss.Compiler.CodeGeneration
{
public class CodeDomGenerationVisitor : IASTVisitor<CodeObject>
{
    private const string TheNamespaceName = "Modeling";
    private const string TheClassName = "TheModel";
    private const string SimulationFieldName = "simulation";


    //private ASTModel model;
    private MetadataRetriever metadataRetriever;
    private Dictionary<string, int> namedVars = new Dictionary<string, int>();
    private HashSet<String> snaMethods = new HashSet<string>();

    private CodeNamespace theNamespace;
    private CodeTypeDeclaration theClass;
    private CodeConstructor theConstructor;

    private int currentBlockNo = 1;
    //private int currentCommandNo = 1;
    private int verbNo = 1;
    private int currentNamedVarNo = 10000;

    public CodeDomGenerationVisitor(MetadataRetriever metadataRetriever)
    {
        this.metadataRetriever = metadataRetriever;

        this.theNamespace = new CodeNamespace();
        this.theNamespace.Name = CodeDomGenerationVisitor.TheNamespaceName;

        this.theClass = new CodeTypeDeclaration();
        this.theClass.Attributes = MemberAttributes.Public;
        this.theClass.Name = CodeDomGenerationVisitor.TheClassName;
        this.theClass.BaseTypes.Add(typeof(AbstractModel));
        this.theNamespace.Types.Add(this.theClass);

        //
        this.theNamespace.Types.Add(RunnableClassGenerator.ConstructRunnableClass(TheClassName));
        //

        this.theConstructor = new CodeConstructor();
        this.theConstructor.Attributes = MemberAttributes.Public;
        this.theClass.Members.Add(this.theConstructor);
    }

    public CodeCompileUnit VisitAll(ASTModel model)
    {
        //return (CodeTypeDeclaration)
        model.Accept(this);

        CodeCompileUnit theAssembly = new CodeCompileUnit();
        // theAssembly.ReferencedAssemblies.AddRange(this.metadataRetriever.AsssemblyPaths.ToArray());
        theAssembly.Namespaces.Add(theNamespace);

        return theAssembly;
    }

    public CodeTypeDeclaration VisitAll_(ASTModel model)
    {
        return (CodeTypeDeclaration)model.Accept(this);
    }

    public CodeObject Visit(ASTModel model)
    {
        // Verbs
        foreach (var verb in model.Verbs)
        {
            // Add comment
            this.theConstructor.Statements.Add(new CodeCommentStatement(String.Empty));
            this.theConstructor.Statements.Add(new CodeCommentStatement(verb.ToString()));

            this.theConstructor.Statements.Add((CodeStatement)verb.Accept(this));
            this.verbNo++;
        }

        return this.theClass;
    }

    public CodeObject Visit(ASTVerb verb)
    {
        Type verbType = this.metadataRetriever.GetVerbType(verb.VerbId);

        // Construct always true if block to wrap several statements
        CodeConditionStatement result = new CodeConditionStatement();
        result.Condition = new CodePrimitiveExpression(true);

        // Process label if exists
        if (verb.LabelId != null)
        {
            // If namedVar is not exists already
            if (!this.namedVars.ContainsKey(verb.LabelId))
            {
                this.CreateNewNamedVar(verb.LabelId);
            }

            // verb is Block
            if (typeof(AnyBlock).IsAssignableFrom(verbType))
            {
                this.ReplaceExistingNamedVar(verb.LabelId);
            }
        }

        // Construct verb declaration
        CodeVariableDeclarationStatement verbDeclaration = new CodeVariableDeclarationStatement();
        verbDeclaration.Type = new CodeTypeReference(verbType);
        verbDeclaration.Name = "verb";
        verbDeclaration.InitExpression = this.CreateConstructorCallExpression(verbType, verb.Operands);
        result.TrueStatements.Add(verbDeclaration);

        // Construct Set label Id for Commands
        if (verb.LabelId != null)
        {
            if (typeof(AnyCommand).IsAssignableFrom(verbType))
            {
                result.TrueStatements.Add(GenerationUtils.ConstructCallCommandSetIdMethod("verb", verb.LabelId));
            }
        }


        // Construct AddVerb method call
        result.TrueStatements.Add(GenerationUtils.ConstructCallAddVerbMethod("verb"));


        // This is needed for Block labels assignment!!!
        if (typeof(AnyBlock).IsAssignableFrom(verbType))
        {
            this.currentBlockNo++;
        }

        return result;
    }

    private void CreateNewNamedVar(string namedVarName)
    {
        int namedVarId = this.currentNamedVarNo;
        this.namedVars.Add(namedVarName, this.currentNamedVarNo);
        this.currentNamedVarNo++;

        // Construct NamedVar field
        this.theClass.Members.Add(GenerationUtils.ConstructCreateNamedVar(namedVarName));

        // Assign namedVar
        this.theConstructor.Statements.Add(GenerationUtils.ConstructAssignNamedVar(namedVarName, namedVarId));

        // Put namedVar to Dictionary
        this.theConstructor.Statements.Add(GenerationUtils.ConstructCallAddNameMethod(namedVarName));
    }

    private void ReplaceExistingNamedVar(string namedVarName)
    {
        int blockNo = this.currentBlockNo;

        // Assign namedVar
        this.theConstructor.Statements.Add(GenerationUtils.ConstructAssignNamedVar(namedVarName, blockNo));

        // Replace namedVar in Dictionary
        this.theConstructor.Statements.Add(GenerationUtils.ConstructCallReplaceNameIdMethod(namedVarName));
    }

    private CodeObjectCreateExpression CreateConstructorCallExpression(Type verbType, IList<ASTAnyExpression> operands)
    {
        CodeObjectCreateExpression constructorCall = new CodeObjectCreateExpression();
        constructorCall.CreateType = new CodeTypeReference(verbType);

        // TODO: Figure out something with first
        foreach (var ctorParam in verbType.GetConstructors().First().GetParameters())
        {
            if (operands.Count > ctorParam.Position && operands[ctorParam.Position] != null)
            {
                ASTAnyExpression operand = operands[ctorParam.Position];

                if (ctorParam.ParameterType == typeof(IDoubleOperand))
                {
                    if (operand is ASTLiteral)
                    {
                        // Operand is a constant
                        ASTLiteral literalOperand = (operand as ASTLiteral);

                        if (literalOperand.LiteralType == LiteralType.Double)
                        {
                            constructorCall.Parameters.Add(GenerationUtils.ConctructCtorNumberParameter((double)literalOperand.Value));
                        }
                        else
                        {
                            throw new CompilerException("Only double operands supported for the moment: " + operand.GetType().Name);
                        }
                    }
                    else
                    {
                        // Operand is an expression

                        string parameterMethodName = String.Format("Verb{0}_Operand{1}", this.verbNo, ctorParam.Position + 1);

                        // Visit expression
                        CodeExpression operandExpression = (CodeExpression)operand.Accept(this);

                        // Construct method for operand
                        this.CreateOperandMethod(parameterMethodName, operandExpression);

                        // Add method delegate as constructor parameter
                        constructorCall.Parameters.Add(GenerationUtils.ConctructCtorDelegateParameter(parameterMethodName));
                }
            }
                else if (ctorParam.ParameterType == typeof(LiteralOperand))
                {
                    if (operand is ASTLValue)
                    {
                        string literal = (operand as ASTLValue).Id;
                        constructorCall.Parameters.Add(GenerationUtils.ConstructCtorLiteralParameter(literal));
                    }
                    else
                    {
                        throw new CompilerException("Wrong AST node for Literal operand: " + operand.GetType().Name);
                    }
                }
                else
                {
                    throw new CompilerException("Not supported parameter type: " + ctorParam.ParameterType.Name);
                }
            }
            else
            {
                // Set null for null operand and for remaining operands
                constructorCall.Parameters.Add(new CodePrimitiveExpression(null));
            }
        }

        return constructorCall;
    }

    private void CreateOperandMethod(string methodName, CodeExpression codeExpr)
    {
        CodeMemberMethod method = new CodeMemberMethod()
        {
            Attributes = MemberAttributes.Public | MemberAttributes.Final,
            ReturnType = new CodeTypeReference(typeof(Double)),
            Name = methodName
        };

        method.Statements.Add(
            new CodeVariableDeclarationStatement(
                typeof(Double), 
                "result"
            )
        );

        method.Statements.Add(
            new CodeAssignStatement(
                new CodeVariableReferenceExpression("result"),
                codeExpr
            )
        );

        method.Statements.Add(
            new CodeMethodReturnStatement(
                new CodeVariableReferenceExpression("result")
            )
        );

        this.theClass.Members.Add(method);
    }

    public CodeObject Visit(ASTBinaryExpression expr)
    {
        CodeBinaryOperatorExpression result = new CodeBinaryOperatorExpression(
            (CodeExpression)expr.Left.Accept(this),
            GenerationUtils.MapBinaryOperator(expr.Operator),
            (CodeExpression)expr.Right.Accept(this)
        );

        return result;
    }

    public CodeObject Visit(ASTLValue lval)
    {
        // If namedVar is not exists already
        if (!this.namedVars.ContainsKey(lval.Id))
        {
            this.CreateNewNamedVar(lval.Id);
        }

        return new CodeFieldReferenceExpression(
            new CodeThisReferenceExpression(),
            lval.Id
        );
    }

    public CodeObject Visit(ASTDirectSNACall sna)
    {
        // If namedVar is not exists already
        if (!this.namedVars.ContainsKey(sna.ActualId))
        {
            this.CreateNewNamedVar(sna.ActualId);
        }

        MethodInfo theMethod = this.metadataRetriever.GetFunctionMethod(sna.SnaId);
        //*
        CodeMethodInvokeExpression result = new CodeMethodInvokeExpression(
            new CodeMethodReferenceExpression(
                new CodeTypeReferenceExpression(theMethod.ReflectedType),
                theMethod.Name
            ),
            new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                CodeDomGenerationVisitor.SimulationFieldName
            ),
            new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                sna.ActualId
            )
        );
        //*/

        /*
        CodeMethodInvokeExpression result = new CodeMethodInvokeExpression(
            new CodeMethodReferenceExpression(
                new CodeThisReferenceExpression(),
                sna.SnaId
            ),
            new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(),
                sna.ActualId
            )
        );

        if (!this.snaMethods.Contains(sna.SnaId))
        {
            this.snaMethods.Add(sna.SnaId);
            this.theClass.Members.Add(GenerationUtils.ConstructSimpleSnaMethod(sna.SnaId));
        }
        //*/
        return result;
    }

    public CodeObject Visit(ASTProcedureCall call)
    {
        MethodInfo theMethod = this.metadataRetriever.GetFunctionMethod(call.ProcedureId);

        CodeMethodInvokeExpression result = new CodeMethodInvokeExpression(
            new CodeMethodReferenceExpression(
                new CodeTypeReferenceExpression(theMethod.ReflectedType),
                theMethod.Name
            )
        );

        foreach (var actual in call.Actuals)
        {
            result.Parameters.Add((CodeExpression)actual.Accept(this));
        }

        return result;
    }

    public CodeObject Visit(ASTLiteral literal)
    {
        // TODO: Temp hack for ints
        if (literal.LiteralType == LiteralType.Int32)
        {
            IConvertible convertible = literal.Value as IConvertible;

            return new CodePrimitiveExpression(convertible.ToDouble(null));
        }
        else
        {
            return new CodePrimitiveExpression(literal.Value);
        }
            
    }
}
}
