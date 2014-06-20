using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Reflection.Emit;

namespace MyAss.Compiler
{
    public class CodeGenerator
    {
        private Dictionary<string, List<double>> ast = new Dictionary<string, List<double>>();

        public CodeGenerator()
        {
            this.ast.Add("Generate", new List<double> { 10 });
        }

        public void Some(string moduleName = "Test.exe")
        {
            AssemblyName assemblyName = new AssemblyName(Path.GetFileNameWithoutExtension(moduleName));
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Save);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName, true);
            TypeBuilder typeBuilder = moduleBuilder.DefineType("MyAss.Generated.Model", TypeAttributes.Public);

            typeBuilder.DefineField("simulationObject", typeof(String), FieldAttributes.Private);


            ILGenerator il;

            MethodBuilder createSimpleMethod = typeBuilder.DefineMethod(
                "SimpleMethod", MethodAttributes.Private | MethodAttributes.Static, typeof(double), new Type[0]);
            il = createSimpleMethod.GetILGenerator();
            LocalBuilder val = il.DeclareLocal(typeof(Int32));
            val.SetLocalSymInfo("block1");
            il.Emit(OpCodes.Ldc_I4, 5);
            il.Emit(OpCodes.Stloc, val);
            il.Emit(OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new System.Type[] {}));
            il.Emit(OpCodes.Ldloc, val);
            //il.Emit(OpCodes.Ldloc, block);
            il.Emit(OpCodes.Ret);

            MethodBuilder createBlocksMethod = typeBuilder.DefineMethod(
                "DefineBlocks", MethodAttributes.Private | MethodAttributes.Static, typeof(double), new Type[0]);
            il = createBlocksMethod.GetILGenerator();
            LocalBuilder block = il.DeclareLocal(typeof(MyAss.Framework.Blocks.Generate));
            block.SetLocalSymInfo("block1");
            il.Emit(OpCodes.Ldc_I4, 5);
            il.Emit(OpCodes.Stloc, block);
            il.Emit(OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new System.Type[] { }));
            il.Emit(OpCodes.Ldloc, block);
            //il.Emit(OpCodes.Ldloc, block);
            il.Emit(OpCodes.Ret);

            MethodBuilder mainMethod = typeBuilder.DefineMethod(
                "Main", MethodAttributes.Public | MethodAttributes.Static, typeof(void), System.Type.EmptyTypes);
            il = mainMethod.GetILGenerator();
            il.Emit(OpCodes.Ldstr, "Test!");
            il.Emit(OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new System.Type[] { typeof(string) }));
            il.EmitCall(OpCodes.Call, createBlocksMethod, new Type[0]);
            il.Emit(OpCodes.Ret);



            typeBuilder.CreateType();
            moduleBuilder.CreateGlobalFunctions();
            assemblyBuilder.SetEntryPoint(mainMethod);
            assemblyBuilder.Save(moduleName);
        }
    }
}
