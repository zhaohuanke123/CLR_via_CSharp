using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace TestNewT
{
    public class Program
    {
        private static int i;

        public static void Main(string[] args)
        {
            _ = BenchmarkDotNet.Running.BenchmarkRunner.Run<Program>();
        }

        [Benchmark(Baseline = true)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestNewInt()
        {
            i = 10;
            DoSameThing(i);
        }

        [Benchmark]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void TestNewTInt()
        {
            i = Activator.CreateInstance<int>();
            DoSameThing(i);
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void DoSameThing(int i)
        {
        }
    }
}