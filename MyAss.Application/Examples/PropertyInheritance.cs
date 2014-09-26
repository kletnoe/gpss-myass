using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Application.Examples
{
    interface I
    {
        int MyProperty { get; }
    }

    abstract class A : I
    {
        public abstract int MyProperty { get; protected set; }
    }

    class C : A
    {
        public override int MyProperty { get; protected set; }
    }
}
