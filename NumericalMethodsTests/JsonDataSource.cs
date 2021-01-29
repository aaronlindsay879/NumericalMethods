using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumericalMethods.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NumericalMethodsTests
{
    [AttributeUsage(AttributeTargets.Method)]
    class JsonDataSource : Attribute, ITestDataSource
    {
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            //read json data as string
            string jsonString = File.ReadAllText("tests.json");

            //parse the json file, and select the correct tests for the method
            var tests = JsonDocument.Parse(jsonString).RootElement
                                    .GetProperty(methodInfo.Name);

            //parse each element to object array and return
            return tests.EnumerateArray()
                        .Select(ParseElement);
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return (string)data.Aggregate((a, b) => $"{a}, {b}");
        }

        /// <summary>
        /// Parses a JsonElement, returning data needed for a test
        /// </summary>
        /// <param name="element">JsonElement to parse</param>
        /// <returns>An object array containing test data</returns>
        private static object[] ParseElement(JsonElement element)
        {
            //find the inputs, and then initialise the output array with correct length
            JsonElement inputs = element.GetProperty("input");
            bool inputIsArray = inputs.ValueKind == JsonValueKind.Array;
            object[] outObj = new object[inputIsArray ? 1 + inputs.GetArrayLength() : 2];

            //set first object to the expected output
            outObj[0] = element.GetProperty("output").Parse();

            //if the input is an array, iterate through it and parse. otherwise just parse singular value
            if (inputIsArray)
            {
                //iterate through all inputs, setting correct type
                //parse either parses as enum, if valid, or as an object
                int i = 1;
                foreach (var subElem in inputs.EnumerateArray())
                    outObj[i++] = subElem.Parse();
            }
            else
                outObj[1] = inputs.Parse();

            return outObj;
        }
    }

    static class JsonElementExtensions
    {
        /// <summary>
        /// Gets the value from a JsonElement as an object, regardless of what type is being stored in it.
        /// This is useful for populating object[] with test data
        /// </summary>
        /// <param name="element">Element to handle</param>
        /// <returns>Value of element as object</returns>
        private static object GetObject(this JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.GetDouble(),
                _ => default
            };
        }

        /// <summary>
        /// Parses a JsonElement, converting to an enum if required (otherwise returns as an object0
        /// </summary>
        /// <param name="element">Element to parse</param>
        /// <returns>Parsed data</returns>
        public static object Parse(this JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.String
                && Enum.IsDefined(typeof(Operators), element.GetString()))
            {
                //if the data is a string, and there is an operator with that name, return as operator
                return Enum.Parse(typeof(Operators), element.GetString());
            }
            else
            {
                //otherwise return as object
                return element.GetObject();
            }
        }
    }
}
