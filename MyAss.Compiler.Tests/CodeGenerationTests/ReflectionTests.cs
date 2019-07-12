using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MyAss.Compiler.Tests.CodeGenerationTests
{
    [TestFixture]
    [Category("ReflectionTests")]
    class ReflectionTests
    {
        [Test]
        public void TestLoadByRelative()
        {
            var asm = Assembly.LoadFrom("MyAss.Framework.BuiltIn.dll");
            Console.WriteLine(asm.FullName);
        }

        [Test]
        public void TestLoadByFullQual()
        {
            //MyAss.Framework.BuiltIn, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
            var asm = Assembly.Load("MyAss.Framework.BuiltIn, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            Console.WriteLine(asm.FullName);
        }
    }
}
