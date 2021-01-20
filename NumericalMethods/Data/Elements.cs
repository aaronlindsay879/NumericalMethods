using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NumericalMethods.Data
{
    /// <summary>
    /// Represents a generic element
    /// </summary>
    public abstract class Element { }

    /// <summary>
    /// Represents an operator, such as plus
    /// </summary>
    public sealed class Operator : Element
    {
        public Operators Op;

        public Operator(Operators op)
        {
            Op = op;
        }

        /// <summary>
        /// Gets the precedence of the operator
        /// </summary>
        /// <returns>The operator</returns>
        public int Precedence() => Op.Precedence();

        /// <summary>
        /// Squashes a double array and an operator into a single value
        /// </summary>
        /// <param name="operators">Operator to use</param>
        /// <param name="inputs">Double array to squash</param>
        /// <returns>Single value</returns>
        public double SquashInput(double[] inputs) => Op.SquashInput(inputs);

        public override string ToString() => Op.Symbol().ToString();
    }

    /// <summary>
    /// Represents a value, such as 4
    /// </summary>
    public sealed class Value : Element
    {
        public double Val;

        public Value(double value)
        {
            Val = value;
        }

        public override string ToString() => Val.ToString();
    }

    /// <summary>
    /// Represents a variable - either unknown (like x), or known (like pi)
    /// </summary>
    public sealed class Variable : Element
    {
        public string Symbol;

        public Variable(string symbol)
        {
            Symbol = symbol;
        }

        public override string ToString() => Symbol.ToString();

        /// <summary>
        /// Gets the value of known variables
        /// </summary>
        /// <returns>Value of a known variable</returns>
        public double? GetValue()
        {
            return Symbol switch
            {
                "e" => Math.E,
                "pi" => Math.PI,
                _ => null
            };
        }
    }
}
