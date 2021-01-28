using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumericalMethods.Data;
using NumericalMethods.NumericalFunctions;
using NumericalMethodsTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods.NumericalFunctions.Tests
{
    [TestClass]
    public class DiffEquationsTests
    {
        [TestMethod, JsonDataSource]
        public void EulersTests(double expected, double step, double x, double y, double iterations, string function)
        {
            Queue<Element> calculated = RPN.SYA(function);
            double value = DiffEquations.Eulers(calculated, step, x, y, (int)iterations);

            Assert.AreEqual(expected, value, 1e-3);
        }
    }
}