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
    public class IntegrationTests
    {
        [TestMethod, JsonDataSource]
        public void MidOrdinateTests(double? expected, double lowerBound, double upperBound, double numOrdinates, string function)
        {
            Queue<Element> calculated = RPN.SYA(function);
            double? value = Integration.MidOrdinate(calculated, lowerBound, upperBound, numOrdinates);

            if (expected.HasValue)
                Assert.AreEqual(expected.Value, value.Value, 1e-3);
            else
                Assert.AreEqual(expected, value);
        }

        [TestMethod, JsonDataSource]
        public void SimpsonsTests(double? expected, double lowerBound, double upperBound, double numOrdinates, string function)
        {
            Queue<Element> calculated = RPN.SYA(function);
            double? value = Integration.Simpsons(calculated, lowerBound, upperBound, numOrdinates);

            if (expected.HasValue)
                Assert.AreEqual(expected.Value, value.Value, 1e-3);
            else
                Assert.AreEqual(expected, value);
        }
    }
}