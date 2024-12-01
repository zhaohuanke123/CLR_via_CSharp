```cs
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

```

.Net Framework 下面

| Method      |      Mean |     Error |    StdDev | Ratio | RatioSD |
| ----------- | --------: | --------: | --------: | ----: | ------: |
| TestNewInt  |  1.128 ns | 0.0005 ns | 0.0004 ns |  1.00 |    0.00 |
| TestNewTInt | 49.632 ns | 0.1678 ns | 0.1401 ns | 44.02 |    0.12 |
|             |           |           |           |       |         |

.Net 6  

| Method      | Mean      | Error     | StdDev    | Ratio |
|------------ |----------:|----------:|----------:|------:|
| TestNewInt  | 0.9667 ns | 0.0010 ns | 0.0009 ns |  1.00 |
| TestNewTInt | 0.9650 ns | 0.0005 ns | 0.0004 ns |  1.00 |
