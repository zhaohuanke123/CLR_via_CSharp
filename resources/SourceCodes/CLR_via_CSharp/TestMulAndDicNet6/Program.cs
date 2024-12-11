using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace TestMulAndDicNet6
{
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkDotNet.Running.BenchmarkRunner.Run<Program>();
        }

        [Benchmark(Baseline = true)]
        public void TestMul()
        {
            var t = 123L;
            float a = 10;
            Mul(a);
        }

        [Benchmark]
        public void TestDiv()
        {
            float a = 10;
            Div(a);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static float Mul(float a)
        {
            return a * (2.0f / 3);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static float Div(float a)
        {
            return a / 1.5f;
        }
    }
}