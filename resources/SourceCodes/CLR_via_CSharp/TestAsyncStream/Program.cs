namespace TestAsyncStream
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            TestAsyncStream().Wait();
        }

        static async Task TestAsyncStream()
        {
            // await foreach
            await foreach (var number in RangeAsync(0, 5, 500))
            {
                Console.WriteLine(number);
            }
        }

        static async IAsyncEnumerable<int> RangeAsync(int start, int count, int delay)
        {
            for (int i = start; i < start + count; i++)
            {
                // 内部return i
                await Task.Delay(delay);
                yield return i;
            }
        }
    }
}