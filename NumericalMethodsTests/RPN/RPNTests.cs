using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumericalMethods;
using NumericalMethods.Data;
using NumericalMethodsTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods.Tests
{
    [TestClass]
    public class RPNTests
    {
        [TestMethod, JsonDataSource]
        public void SYATests(string expected, string input)
        {
            Queue<Element> calculated = RPN.SYA(input);

            Assert.AreEqual(expected, RPN.ToString(calculated));
        }
    }
}