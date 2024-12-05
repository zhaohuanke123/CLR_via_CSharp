namespace TestSpan
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Running;
    using System;

    public class StringComparisonBenchmark
    {
        private string str1;
        private string str2;

        public StringComparisonBenchmark()
        {
            str1 = "This is a test string for benchmarking.";
            str2 = "This is a test string for benchmarking.";
        }

        // 使用Span进行字符串比较
        [Benchmark]
        public bool CompareWithSpan()
        {
            ReadOnlySpan<char> span1 = str1.AsSpan();
            ReadOnlySpan<char> span2 = str2.AsSpan();

            return span1.SequenceEqual(span2);
        }

        [Benchmark]
        public int CompareEqual()
        {
            return String.Compare(str1, str2, StringComparison.Ordinal);
        }

        // 使用逐字符对比
        [Benchmark]
        public bool CompareCharByChar()
        {
            if (str1.Length != str2.Length)
                return false;

            for (int i = 0; i < str1.Length; i++)
            {
                if (str1[i] != str2[i])
                    return false;
            }

            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<StringComparisonBenchmark>();
        }
    }
}