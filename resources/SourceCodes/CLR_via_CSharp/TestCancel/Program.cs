using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestCancel
{
    internal static class Cancellation
    {
        public static async Task Go()
        {
            // Create a CancellationTokenSource that cancels itself after # milliseconds
            var cts = new CancellationTokenSource(900); // To cancel sooner, call cts.Cancel()
            var ct = cts.Token;

            try
            {
                await Task.Delay(1000).WithCancellation(ct); // 模拟一个任务，等待 1000 毫秒
                Console.WriteLine("Task completed");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Task cancelled");
            }
        }

        private static async Task Run()
        {
            await Task.Delay(1000);
            Console.WriteLine("Task completed");
        }

        private struct Void
        {
        } // Because there isn't a non-generic TaskCompletionSource class.

        private static async Task<TResult> WithCancellation<TResult>(this Task<TResult> orignalTask,
            CancellationToken ct)
        {
            // Create a Task that completes when the CancellationToken is canceled
            var cancelTask = new TaskCompletionSource<Void>();

            // When the CancellationToken is cancelled, complete the Task
            using (ct.Register(t => ((TaskCompletionSource<Void>)t).TrySetResult(new Void()), cancelTask))
            {
                // Create another Task that completes when the original Task or when the CancellationToken's Task
                Task any = await Task.WhenAny(orignalTask, cancelTask.Task);

                // If any Task completes due to CancellationToken, throw OperationCanceledException         
                if (any == cancelTask.Task) ct.ThrowIfCancellationRequested();
            }

            // await original task (synchronously); if it failed, awaiting it 
            // throws 1st inner exception instead of AggregateException
            return await orignalTask;
        }

        private static async Task WithCancellation(this Task task, CancellationToken ct)
        {
            var tcs = new TaskCompletionSource<Void>();
            using (ct.Register(t =>
                       {
                           var taskCompletionSource = (TaskCompletionSource<Void>)t;
                           taskCompletionSource.TrySetResult(default);
                       },
                       tcs))
            {
                if (await Task.WhenAny(task, tcs.Task) == tcs.Task)
                {
                    ct.ThrowIfCancellationRequested();
                }
            }

            await task; // If failure, ensures 1st inner exception gets thrown instead of AggregateException
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            Cancellation.Go().Wait();
        }
    }
}