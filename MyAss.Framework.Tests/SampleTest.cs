using System;
using NUnit.Framework;

namespace MyAss.Framework.Tests
{
    [TestFixture]
    public class SampleTest
    {
        private Random rand = new Random(1);

        [Test]
        public void SomeTest()
        {
            Console.WriteLine("Hello test!");
            Assert.True(true);
        }
    }
}
