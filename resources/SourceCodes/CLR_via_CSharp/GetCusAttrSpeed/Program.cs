using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace GetCusAttrSpeed
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<Program>();
        }

        [Benchmark]
        public void Test3()
        {
            var type = typeof(Program);
            type.GetCustomAttributes(true);
            type.GetCustomAttributes(true);
            type.GetCustomAttributes(true);
            type.GetCustomAttributes(true);
        }

        [Benchmark(Baseline = true)]
        public void Test2()
        {
            var type = typeof(Program);
            type.GetCustomAttributes(true);
            type.GetCustomAttributes(true);
        }
    }
}