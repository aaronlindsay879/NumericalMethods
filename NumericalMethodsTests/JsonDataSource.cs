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
            //find the inputs and outputs
            object[] inputs = element.GetProperty("input").Parse();
            object[] outputs = element.GetProperty("output").Parse();

            //then concat and return
            return outputs.Concat(inputs).ToArray();
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
            //if the element is an enum, parse and return
            if (element.ValueKind == JsonValueKind.String
                && Enum.IsDefined(typeof(Operators), element.GetString()))
            {
                return Enum.Parse(typeof(Operators), element.GetString());
            }

            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.GetDouble(),
                _ => default
            };
        }

        /// <summary>
        /// Enumerates a json element and applies a given select function
        /// </summary>
        /// <param name="element">Element to enumerate+select</param>
        /// <param name="func">Function to apply</param>
        /// <returns>Object array</returns>
        private static object[] EnumerateSelect(this JsonElement element, Func<JsonElement, object> func) =>
            element.EnumerateArray().Select(func).ToArray();

        /// <summary>
        /// Parses a JsonElement, converting each element to an enum if required (otherwise returns as an object).
        /// If the element is an array, all children are parsed individually
        /// </summary>
        /// <param name="element">Element to parse</param>
        /// <returns>Parsed data in an array</returns>
        public static object[] Parse(this JsonElement element)
        {
            //if the element isn't an array, simply return object
            if (element.ValueKind != JsonValueKind.Array)
                return new object[] { element.GetObject() };

            //otherwise parse each object
            return element.EnumerateSelect(x => x.GetObject());
        }
    }
}
