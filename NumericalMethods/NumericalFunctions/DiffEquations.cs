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
    }
}
