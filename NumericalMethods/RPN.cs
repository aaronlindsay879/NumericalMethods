﻿using NumericalMethods.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NumericalMethods
{
    public class RPN
    {
        /// <summary>
        /// Parses an input, splitting into tokens
        /// </summary>
        /// <param name="input">Input to split</param>
        /// <returns>Tokens representing original input</returns>
        private static IEnumerable<string> ParseInput(string input)
        {
            //remove all whitespace from input
            input = new(input.Where(x => !char.IsWhiteSpace(x)).ToArray());

            //split on "^", "/" and "(" to "-" (all operators used in this program)
            return Regex.Split(input, @"(\^|/|[\(-\-])").Where(x => !string.IsNullOrWhiteSpace(x));
        }

        /// <summary>
        /// Performs the shunting-yard algorithm on an input to convert from infix to postfix notation
        /// </summary>
        /// <param name="input">String input in infix notation</param>
        /// <returns>Queue of elements in postfix notation</returns>
        public static Queue<Element> SYA(string input)
        {
            List<string> tokens = ParseInput(input).ToList();
            Stack<Operator> opStack = new();
            Queue<Element> outQueue = new();

            foreach (string token in tokens)
            {
                //if the token is a number, simply add to queue
                if (int.TryParse(token, out int number))
                    outQueue.Enqueue(new Value(number));

                //if the token is an operator
                else if (OperatorsHelper.TryParse(token, out Operators op) && op != Operators.LeftBracket && op != Operators.RightBracket)
                {
                    //move operators from operator stack to output queue while the top element in operator stack has greater precedence
                    while (opStack.Count > 0 && opStack.Peek().Precedence() > op.Precedence())
                        outQueue.Enqueue(opStack.Pop());

                    //once all operators of greater precedence gone, add operator to operator queue
                    opStack.Push(new Operator(op));
                }

                //if the operator is a left bracket, just push onto opstack
                else if (op == Operators.LeftBracket)
                    opStack.Push(new Operator(op));

                //if right bracket, move operators from opstack to outqueue until left bracket is found
                //then discard both left and right bracket
                else if (op == Operators.RightBracket)
                {
                    while (opStack.Peek().Op != Operators.LeftBracket)
                        outQueue.Enqueue(opStack.Pop());

                    opStack.Pop();
                }

                else
                    outQueue.Enqueue(new Variable(token));
            }

            //once all tokens are done, move all operators over to output queue
            while (opStack.Count > 0)
                outQueue.Enqueue(opStack.Pop());

            return outQueue;
        }

        /// <summary>
        /// Computes the value of a parsed expression
        /// </summary>
        /// <param name="elements">Parsed expression</param>
        /// <returns>Value</returns>
        public static double Compute(Queue<Element> elements, double variableValue = 0)
        {
            //generate a copy of the input queue to work on, and create a workstack
            Queue<Element> workQueue = new(elements);
            Stack<Element> workStack = new();

            //while there are still items in the queue
            while (workQueue.Count > 0)
            {
                //take the top element
                Element elem = workQueue.Dequeue();

                //if the element is a value, simply push to work stack
                if (elem is Value val)
                    workStack.Push(val);
                else if (elem is Variable var)
                {
                    Value variableVal;

                    //if the variable is x, replace with value given when function called
                    //otherwise lookup value of variable
                    if (var.Symbol == "x")
                        variableVal = new Value(variableValue);
                    else
                        variableVal = new Value(var.GetValue() ?? 0d);

                    //then push to stack
                    workStack.Push(variableVal);
                }
                else if (elem is Operator op)
                {
                    //if the element is an operator, get one or two values from the work stack
                    List<double> values = new();

                    int numInputs = Math.Min(2, op.Op.Inputs());
                    while (workStack.Count > 0
                        && values.Count < numInputs
                        && workStack.Peek().GetType() == typeof(Value))
                    {
                        values.Add((workStack.Pop() as Value).Val);
                    }

                    //then "squash" those values with the operator to create a new value, which is pushed to work stack
                    workStack.Push(new Value(op.SquashInput(values.ToArray())));
                }
            }

            //once only one item remains, return the value of it
            return (workStack.Pop() as Value).Val;
        }
    }
}