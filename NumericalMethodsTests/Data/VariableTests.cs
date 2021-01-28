using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumericalMethods.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumericalMethodsTests;

namespace NumericalMethods.Data.Tests
{
    [TestClass]
    public class VariableTests
    {
        [TestMethod, JsonDataSource]
        public void GetValueTests(double expected, string symbol)
        {
            Variable var = new(symbol);

            Assert.AreEqual(expected, var.GetValue().Value, 1e-6);
        }
    }
}