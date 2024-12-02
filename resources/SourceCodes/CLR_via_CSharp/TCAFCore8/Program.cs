using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace TCAFCore
{
    public class Program
    {
        static string name = "Tom";
        static int age = 18;

        public static void Main(string[] args)
        {
            _ = BenchmarkRunner.Run<Program>();
        }

        [Benchmark]
        public void TestFormat()
        {
            _ = ($"My name is {name}, I'm {age} years old.");
        }

        [Benchmark]
        public void TestConcat()
        {
            _ = ("My name is " + name + ", I'm " + age + " years old.");
        }
    }
}