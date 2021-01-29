using NumericalMethods.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NumericalMethods.NumericalFunctions
{
    public static class DiffEquations
    {
        public static double Eulers(Queue<Element> elements, double step, double x, double y, int iterations)
        {
            for (int i = iterations; i > 0; i--)
            {
                y += step * RPN.Compute(elements, ("x", x), ("y", y));
                x += step;
            }

            return y;
        }

        public static double ImprovedEulers(Queue<Element> elements, double step, double x, List<double> y, int iterations)
        {
            if (y.Count == 1)
            {
                y.Add(Eulers(elements, step, x, y[0], 1));
                x += step;
                iterations--;
            }

            for (int i = iterations; i > 0; i--)
            {
                y.Add(y[^2] + 2 * step * RPN.Compute(elements, ("x", x), ("y", y[^1])));
                x += step;
            }

            return y[^1];
        }
    }
}
