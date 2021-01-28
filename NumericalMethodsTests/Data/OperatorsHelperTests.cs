using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumericalMethods.Data;
using NumericalMethodsTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods.Data.Tests
{
    [TestClass]
    public class OperatorsHelperTests
    {
        [TestMethod, JsonDataSource]
        public void SquashInputTests(double expected, Operators op, params double[] inputs)
        {
            Assert.AreEqual(expected, op.SquashInput(inputs));
        }

        [TestMethod, JsonDataSource]
        public void TryParseTests(Operators expected, string input)
        {
            OperatorsHelper.TryParse(input, out var op);
            Assert.AreEqual(expected, op);
        }
    }
}