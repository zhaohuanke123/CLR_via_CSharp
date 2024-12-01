namespace TCAFFramework
{
    public class Program
    {
        static string name = "Tom";
        static int age = 18;

        public static void Main(string[] args)
        {
            // BenchmarkDotNet.Running.BenchmarkRunner.Run<Program>();
        }

        // [Benchmark]
        public void TestFormat()
        {
            _ = ($"My name is {name}, I'm {age} years old.");
        }

        // [Benchmark]
        public void TestConcat()
        {
            _ = ("My name is " + name + ", I'm " + age + " years old.");
        }
    }
}