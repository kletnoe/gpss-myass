using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Compiler.Metadata;
using MyAss.Compiler_v2.AST;

namespace MyAss.Compiler_v2.CodeGeneration
{
    public class CompileUnitConstructor
    {
        private const string TheNamespaceName = "Modeling";

        public CodeCompileUnit CompileUnit { get; private set; }

        public CompileUnitConstructor(ASTModel astModel, MetadataRetriever_v2 metadataRetriever)
        {
            CodeDomGenerationVisitor visitor = new CodeDomGenerationVisitor(metadataRetriever);
            CodeTypeDeclaration modelClass = visitor.VisitAll_(astModel);

            CodeNamespace theNamespace = new CodeNamespace();
            theNamespace.Name = CompileUnitConstructor.TheNamespaceName;

            theNamespace.Types.Add(modelClass);
            theNamespace.Types.Add(RunnableClassGenerator.ConstructRunnableClass(modelClass.Name));

            this.CompileUnit = new CodeCompileUnit();
            this.CompileUnit.Namespaces.Add(theNamespace);
        }
    }
}
