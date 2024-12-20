using BenchmarkDotNet.Attributes;

namespace TestSpanNet6
{
    public class Test
    {
        public static double[] Arr => arr;
        static double[] arr = new double[10000];

        [Benchmark(Baseline = true)]
        public void RunSpan()
        {
            Span<double> span = arr;
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = 1;
            }
        }

        [Benchmark]
        public void RunNoSpan()
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = 1;
            }
        }
    }

    internal class Program
    {
        public static void Main()
        {
            BenchmarkDotNet.Running.BenchmarkRunner.Run<Test>();
        }
    }
}