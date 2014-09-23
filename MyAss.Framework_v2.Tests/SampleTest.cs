using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAss.Framework_v2.OperandTypes;
using MyAss.Framework_v2.TablePackage.Entities;
using NUnit.Framework;

namespace MyAss.Framework_v2.Tests
{
    [TestFixture]
    public class SampleTest
    {
        [Test]
        public void SomeTest()
        {
            TableEntity table = new TableEntity(null, 0, new ParExpression(() => 1), 0, 4, 10);

            Assert.Pass(table.GetStandardReportLine());
        }
    }
}
