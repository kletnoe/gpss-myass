using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAss.Application.Examples.CustomAttributes
{
    [System.AttributeUsage(AttributeTargets.Constructor, Inherited = false, AllowMultiple = false)]
    sealed class VerbConstructorAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236

        // This is a positional argument
        public VerbConstructorAttribute()
        {

            // TODO: Implement code here

        }
    }

    public class Test
    {
        [VerbConstructor]
        public Test()
        {

        }

        [VerbConstructor]
        public Test(int i)
        {

        }
    }
}
