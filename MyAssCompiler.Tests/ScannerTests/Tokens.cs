using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MyAssCompiler.Tests.ScannerTests
{
    [TestFixture]
    public class Tokens
    {
        [Test]
        public void Id_Literal_Literal()
        {
            string input = @"Some 1 2";

            string result = CommonCode(input, false);
            Assert.Pass(result);
        }

        [Test]
        public void Id_Literal_Literal_NoSpaces()
        {
            string input = @"1 2 3 4";

            string result = CommonCode(input, true);
            Assert.Pass(result);
        }

        private static string CommonCode(string input, bool ignoreSpaces)
        {
            IScanner scanner = new Scanner(new StringCharSource(input));
            StringBuilder sb = new StringBuilder(Environment.NewLine);

            scanner.IgnoreWhitespace = ignoreSpaces;
            while (scanner.CurrentToken != TokenType.EOF)
            {
                sb.AppendLine(scanner.CurrentToken.ToString() + "\t\t" + scanner.CurrentTokenVal.ToString());
                scanner.Next();
            }

            return sb.ToString();
        }
    }
}
