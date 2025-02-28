using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace TestCancellationToken
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            // var threadPoolSynchronizationContext = new MainThreadSynchronizationContext();
            // SynchronizationContext.SetSynchronizationContext(threadPoolSynchronizationContext);

            // var task = Task.Run(() => { Thread.Sleep(1000); });
            // var taskAwaiter = task.GetAwaiter();
            // taskAwaiter.OnCompleted(() =>
            // {
            //     Console.WriteLine($"OnCompleted {Thread.CurrentThread.ManagedThreadId}");
            // });

            // CancellationTokenSource cts = new CancellationTokenSource();
            // CancellationToken token = cts.Token;

            await TestAsync();
            // Task.Run(() =>
            // {
            //     Thread.Sleep(1000);
            //     Console.Out.WriteLine($"Run in ID : {Thread.CurrentThread.ManagedThreadId}");
            // }, token);
            // token.Register(
            //     () => { Console.Out.WriteLine($"In Register1 ID : {Thread.CurrentThread.ManagedThreadId}"); },
            //     false);
            // token.Register(
            //     () => { Console.Out.WriteLine($"In Register2 ID : {Thread.CurrentThread.ManagedThreadId}"); },
            //     false);
            // token.Register(
            //     () => { Console.Out.WriteLine($"In Register3 ID : {Thread.CurrentThread.ManagedThreadId}"); },
            //     false);
            //
            // Thread.Sleep(1000);
            // cts.Cancel();
            while (true)
            {
                if (actions.TryDequeue(out var action))
                {
                    action();
                }
            }
            // task.Wait();
        }

        private static async Task TestAsync()
        {
            Console.Out.WriteLine($"Run ID : {Thread.CurrentThread.ManagedThreadId}");
            await Task.Run((async () =>
            {
                Console.Out.WriteLine($"Run In ID : {Thread.CurrentThread.ManagedThreadId}");
                await Task.Delay(1000);
                Console.Out.WriteLine($"Run In ID : {Thread.CurrentThread.ManagedThreadId}");
            }));
            Console.Out.WriteLine($"Run ID : {Thread.CurrentThread.ManagedThreadId}");
        }

        public static ConcurrentQueue<Action> actions = new ConcurrentQueue<Action>();
    }

    internal class MainThreadSynchronizationContext : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object state)
        {
            Console.WriteLine($"In Post: {Thread.CurrentThread.ManagedThreadId}");
            // ThreadPool.QueueUserWorkItem(_ => d(state));
            Program.actions.Enqueue(() => d(state));
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            d(state);
        }
    }
}