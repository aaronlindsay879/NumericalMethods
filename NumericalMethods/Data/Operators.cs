using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace NumericalMethods.Data
{
    /// <summary>
    /// Contains info about an operator, which is used for parsing and constructing strings
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class OpInfo : Attribute
    {
        public int Precedence { get; init; }
        public string Symbol { get; init; }
        public int Inputs { get; init; }

        /// <summary>
        /// Gives the operator a given <paramref name="precedence"/> and <paramref name="symbol"/>
        /// </summary>
        /// <param name="precedence">The precedence of the operator - higher has a higher priority</param>
        /// <param name="symbol">The symbol used to represent the operator</param>
        /// <param name="inputs">The number of inputs for a function</param>
        public OpInfo(int precedence, string symbol, int inputs)
        {
            Precedence = precedence;
            Symbol = symbol;
            Inputs = inputs;
        }
    }

    /// <summary>
    /// Contains info about mathematical operators
    /// </summary>
    public enum Operators
    {
        //normal operators
        [OpInfo(4, "^", 2)] Power,
        [OpInfo(3, "*", 2)] Multiply,
        [OpInfo(3, "/", 2)] Divide,
        [OpInfo(2, "+", 2)] Plus,
        [OpInfo(2, "-", 2)] Subtract,

        //special operators
        [OpInfo(0, "(", 2)] LeftBracket,
        [OpInfo(0, ")", 0)] RightBracket,

        //functions
        [OpInfo(5, "sin", 1)] Sin,
        [OpInfo(5, "cos", 1)] Cos,
        [OpInfo(5, "tan", 1)] Tan,
        [OpInfo(5, "sqrt", 1)] Sqrt,
        [OpInfo(5, "neg", 1)] Neg,
        [OpInfo(5, "ln", 1)] Ln,
        [OpInfo(5, "abs", 1)] Abs
    }

    /// <summary>
    /// Extension class to add methods to operators enum
    /// </summary>
    public static class OperatorsHelper
    {
        /// <summary>
        /// Gets the operator info for a given operator
        /// </summary>
        /// <param name="operators">Operator to find info for</param>
        /// <returns>OpInfo for given operator</returns>
        private static OpInfo GetOpInfo(this Operators operators)
        {
            var enumType = operators.GetType();
            var memInfo = enumType.GetMember(operators.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(OpInfo), false);
            return (OpInfo)attributes[0];
        }

        /// <summary>
        /// Finds the precedence for a given operator
        /// </summary>
        /// <param name="operators">Operator to find precedence for</param>
        /// <returns>Precedence</returns>
        public static int Precedence(this Operators operators) => operators.GetOpInfo().Precedence;

        /// <summary>
        /// Finds the symbol for a given operator
        /// </summary>
        /// <param name="operators">Operator to find symbol for</param>
        /// <returns>Symbol</returns>
        public static string Symbol(this Operators operators) => operators.GetOpInfo().Symbol;

        /// <summary>
        /// Finds the inputs for a given operator
        /// </summary>
        /// <param name="operators">Operator to find inputs for</param>
        /// <returns>Inputs</returns>
        public static int Inputs(this Operators operators) => operators.GetOpInfo().Inputs;

        /// <summary>
        /// Squashes a double array and an operator into a single value
        /// </summary>
        /// <param name="operators">Operator to use</param>
        /// <param name="inputs">Double array to squash</param>
        /// <returns>Single value</returns>
        public static double SquashInput(this Operators operators, double[] inputs)
        {
            //unary operators
            if (inputs.Length == 1)
                return operators switch
                {
                    Operators.Neg => -inputs[0],
                    Operators.Sin => Math.Sin(inputs[0]),
                    Operators.Cos => Math.Cos(inputs[0]),
                    Operators.Tan => Math.Tan(inputs[0]),
                    Operators.Sqrt => Math.Sqrt(inputs[0]),
                    Operators.Ln => Math.Log(inputs[0]),
                    Operators.Abs => Math.Abs(inputs[0]),
                    _ => default
                };

            //operators with two args
            if (inputs.Length == 2)
                return operators switch
                {
                    Operators.Plus => inputs[1] + inputs[0],
                    Operators.Subtract => inputs[1] - inputs[0],
                    Operators.Multiply => inputs[1] * inputs[0],
                    Operators.Divide => inputs[1] / inputs[0],
                    Operators.Power => Math.Pow(inputs[1], inputs[0]),
                    _ => default
                };

            return default;
        }

        /// <summary>
        /// Tries to parse a string to an operator
        /// </summary>
        /// <param name="str">String to parse</param>
        /// <param name="operators">Operator variable to write to</param>
        /// <returns></returns>
        public static bool TryParse(string str, out Operators operators)
        {
            operators = new();

            //loop through all enum elements, comparing symbol to (lowercase) string until a match is found
            foreach (Operators enumElement in Enum.GetValues(typeof(Operators)))
            {
                if (enumElement.Symbol() == str.ToLower())
                {
                    operators = enumElement;
                    return true;
                }
            }

            return false;
        }
    }
}
