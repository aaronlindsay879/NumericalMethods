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

        [TestMethod, JsonDataSource]
        public void ComputeTests(double expected, string input, double xValue = 0)
        {
            Queue<Element> elements = new();
            foreach (string token in input.Split(' '))
            {
                if (double.TryParse(token, out double number))
                    elements.Enqueue(new Value(number));
                else if (OperatorsHelper.TryParse(token, out Operators op))
                    elements.Enqueue(new Operator(op));
                else
                    elements.Enqueue(new Variable(token));
            }

            double value = RPN.Compute(elements, xValue);
            Assert.AreEqual(expected, value, 1e-6);
        }

        [TestMethod, JsonDataSource]
        public void RPNIntegrationTests(double expected, string input, double xValue = 0)
        {
            Queue<Element> calculated = RPN.SYA(input);

            Assert.AreEqual(expected, RPN.Compute(calculated, xValue), 1e-6);
        }
    }
}