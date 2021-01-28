using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumericalMethods.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace NumericalMethodsTests
{
    class JsonDataSource : Attribute, ITestDataSource
    {
        private readonly string _filePath;

        public JsonDataSource(string filePath)
        {
            _filePath = filePath;
        }

        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            string jsonString = File.ReadAllText(_filePath);

            return JsonSerializer.Deserialize<List<Data>>(jsonString).Select(x => (object[])x);
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return (string)data.Aggregate((a, b) => $"{a}, {b}");
        }
    }

    public class Data
    {
        public object output { get; set; }
        public object[] inputs { get; set; }

        public static implicit operator object[](Data data)
        {
            Type opType = typeof(Operators);
            object[] outputArr = new object[data.inputs.Length + 1];

            outputArr[0] = double.Parse(data.output.ToString());

            int i = 1;
            foreach (var obj in data.inputs)
                outputArr[i++] = Enum.TryParse(opType, obj.ToString(), out var op) && Enum.IsDefined(opType, obj.ToString())
                                 ? op 
                                 : double.Parse(obj.ToString());

            return outputArr;
        }
    }
}
