using NumericalMethods.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NumericalMethods.NumericalFunctions
{
    public static class DiffEquations
    {
        /// <summary>
        /// Approximates a value for a differential equation using Euler's formula
        /// </summary>
        /// <param name="formula">Differential formula to approximate</param>
        /// <param name="step">How far forwards to go each step</param>
        /// <param name="x">Initial x value</param>
        /// <param name="y">Initial y value</param>
        /// <param name="iterations">How many steps to take</param>
        /// <returns>A double indicating y value at final value of x</returns>
        public static double Eulers(Queue<Element> formula, double step, double x, double y, int iterations)
        {
            for (int i = iterations; i > 0; i--)
            {
                y += step * RPN.Compute(formula, ("x", x), ("y", y));
                x += step;
            }

            return y;
        }

        /// <summary>
        /// Approximates a value for a differential equation using Euler's improved formula
        /// </summary>
        /// <param name="formula">Differential formula to approximate</param>
        /// <param name="step">How far forwards to go each step</param>
        /// <param name="x">Initial x value</param>
        /// <param name="y">Initial y value(s)</param>
        /// <param name="iterations">How many steps to take</param>
        /// <returns>A double indicating y value at final value of x</returns>
        public static double ImprovedEulers(Queue<Element> formula, double step, double x, List<double> y, int iterations)
        {
            //if only one y value is given, calculate a second using euler's formula
            if (y.Count == 1)
            {
                y.Add(Eulers(formula, step, x, y[0], 1));
                x += step;
                iterations--;
            }

            //then compute following values using improved formula
            for (int i = iterations; i > 0; i--)
            {
                y.Add(y[^2] + 2 * step * RPN.Compute(formula, ("x", x), ("y", y[^1])));
                x += step;
            }

            //return last value of y
            return y[^1];
        }
    }
}
