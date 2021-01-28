using NumericalMethods.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NumericalMethods.NumericalFunctions
{
    public static class Integration
    {
        public static double? MidOrdinate(Queue<Element> elements, double lowerBound, double upperBound, double numOrdinates)
        {
            if (lowerBound > upperBound)
                return null;

            double h = (upperBound - lowerBound) / numOrdinates;
            double sum = 0;

            for (double i = lowerBound; i < upperBound; i += h)
                sum += RPN.Compute(elements, i + h / 2);

            sum *= h;

            return sum;
        }

        public static double? Simpsons(Queue<Element> elements, double lowerBound, double upperBound, double numOrdinates)
        {
            if (lowerBound > upperBound)
                return null;

            double h = (upperBound - lowerBound) / numOrdinates;

            double firstSum = RPN.Compute(elements, lowerBound) + RPN.Compute(elements, upperBound);
            double secondSum = 0;
            double thirdSum = 0;
            double sum = 0;

            for (double i = lowerBound + h; i < upperBound; i += 2 * h)
                secondSum += RPN.Compute(elements, i);

            for (double i = lowerBound + 2 * h; i < upperBound; i += 2 * h)
                thirdSum += RPN.Compute(elements, i);

            sum = firstSum + 4 * secondSum + 2 * thirdSum;
            sum *= h / 3;

            return sum;
        }
    }
}
