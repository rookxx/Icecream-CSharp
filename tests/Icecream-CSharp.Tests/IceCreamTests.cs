using System;
using System.IO;
using System.Text;
using Xunit;
using IcDebugger;
using System.Text.RegularExpressions;

namespace Icecream_CSharp.Tests
{
    public class IcDebugger_Test : IDisposable
    {
        private readonly TextWriter originalConsoeOut;
        private StringWriter tempStringWriter;

        public IcDebugger_Test()
        {
            originalConsoeOut = Console.Out;
            tempStringWriter = new StringWriter();
            Console.SetOut(tempStringWriter);
        }

        public void Dispose()
        {
            tempStringWriter.Dispose();
            Console.SetOut(originalConsoeOut);
        }

        private string GetConsoleOutput()
        {
            var result = tempStringWriter.ToString().TrimEnd('\n');
            tempStringWriter.Dispose();
            tempStringWriter = new StringWriter();
            Console.SetOut(tempStringWriter);
            return result ?? string.Empty;
        }

        private class TestClass
        {
            public string Name { get; private set; }
            public int Age { get; private set; }

            public TestClass(string name, int age)
            {
                Name = name;
                Age = age;
            }

            public override string ToString()
                => $"Name = {Name} Age = {Age}";
        }

        [Fact]
        public void Ic_Default_Option_Test()
        {
            IceCream.ConfigureOutput(includeContext: false);
            IceCream.Ic("test1");
            Assert.Equal("Ic | \"test1\":test1", GetConsoleOutput());

            var a = 1;
            IceCream.Ic(a == 1);
            Assert.Equal("Ic | a == 1:True", GetConsoleOutput());

            var b = 2;
            IceCream.Ic(a + b);
            Assert.Equal("Ic | a + b:3", GetConsoleOutput());

            var testClass = new TestClass("rookxx", 40);
            IceCream.Ic(testClass);
            Assert.Equal("Ic | testClass:Name = rookxx Age = 40", GetConsoleOutput());
        }

        [Fact]
        public void Ic_Include_Context_Test()
        {
            IceCream.ConfigureOutput(includeContext: true);

            var regex = new Regex("Ic | (?<filepath>.*?) in (?<method>.*?) line [0-9]* :(.*):(.*)");
            var a = 1;
            IceCream.Ic(a == 1);
            var output = GetConsoleOutput();
            var match = regex.Matches(output);

            Dispose();
            Console.WriteLine(match);
        }
    }
}