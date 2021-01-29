using NumericalMethods.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NumericalMethods.NumericalFunctions
{
    public static class Integration
    {
        /// <summary>
        /// Integrates a function using the mid-ordinate approximation
        /// </summary>
        /// <param name="function">Function to integrate</param>
        /// <param name="lowerBound">Lower bound of integration</param>
        /// <param name="upperBound">Upper bound of integration</param>
        /// <param name="numOrdinates">Number of ordinates to integrate with</param>
        /// <returns>A double? where null means it was an invalid operation</returns>
        public static double? MidOrdinate(Queue<Element> function, double lowerBound, double upperBound, double numOrdinates)
        {
            //if lower bound is higher than upper bound, then it's invalid
            if (lowerBound >= upperBound)
                return null;

            //find the distance between ordinates
            double h = (upperBound - lowerBound) / numOrdinates;
            double sum = 0;

            //compute the function at midpoints between the ordinates, and add up
            for (double i = lowerBound; i < upperBound; i += h)
                sum += RPN.Compute(function, ("x", i + h / 2));

            sum *= h;

            return sum;
        }

        /// <summary>
        /// Integrates a function using simpson's approximation
        /// </summary>
        /// <param name="function">Function to integrate</param>
        /// <param name="lowerBound">Lower bound of integration</param>
        /// <param name="upperBound">Upper bound of integration</param>
        /// <param name="numOrdinates">Number of ordinates to integrate with</param>
        /// <returns>A double? where null means it was an invalid operation</returns>
        public static double? Simpsons(Queue<Element> function, double lowerBound, double upperBound, double numOrdinates)
        {
            //if lower bound is higher than upper bound, then it's invalid
            if (lowerBound >= upperBound)
                return null;

            //find the distance between ordinates
            double h = (upperBound - lowerBound) / numOrdinates;

            //calculate the sum of the first and last ordinate
            double firstSum = RPN.Compute(function, ("x", lowerBound)) + RPN.Compute(function, ("x", upperBound));

            //calculate the sum of the odd ordinates
            double secondSum = 0;
            for (double i = lowerBound + h; i < upperBound; i += 2 * h)
                secondSum += RPN.Compute(function, ("x", i));

            //calculate the sum of the even ordinates
            double thirdSum = 0;
            for (double i = lowerBound + 2 * h; i < upperBound; i += 2 * h)
                thirdSum += RPN.Compute(function, ("x", i));

            //find total sum from the calculated values
            double sum = firstSum + 4 * secondSum + 2 * thirdSum;
            sum *= h / 3;

            return sum;
        }
    }
}
