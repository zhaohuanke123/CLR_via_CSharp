using System.Runtime.CompilerServices;

namespace ValueTask
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            // ValueTask<int> valueTask = GetValueTask();
            // await valueTask; // 第一次等待
            // await valueTask; // 错误：不能再次等待
            //
            // int res = valueTask.GetAwaiter().GetResult();
            // Console.WriteLine(res);
            
            // Yield().Wait();
        }

        private static ValueTask<int> GetValueTask()
        {
            // 同步完成时直接返回结果
            if (SomeCondition())
            {
                return new ValueTask<int>(42);
            }

            // 异步完成时返回 Task
            return new ValueTask<int>(DoAsyncWork());
        }

        private static async Task<int> DoAsyncWork()
        {
            await Task.Delay(1000);
            return 42;
        }

        private static bool SomeCondition()
        {
            return DateTime.Now.Second % 2 == 0;
        }

        public static async ValueTask<int> GetValueTaskAsync()
        {
            await Task.CompletedTask; // 这里别误会，这是随便找个地方 await 一下
            return 666;
        }

        public static async Task<int> GetTaskAsync()
        {
            await Task.CompletedTask;
            return 666;
        }

        private static async void Yield()
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                await Task.Yield();
            }
        }
    }
}