using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestCancellationToken
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // var threadPoolSynchronizationContext = new ThreadPoolSynchronizationContext();
            // SynchronizationContext.SetSynchronizationContext(threadPoolSynchronizationContext);
            var task = Task.Run((() =>
            {
                var threadPoolSynchronizationContext = new ThreadPoolSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(threadPoolSynchronizationContext);
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;

                Console.Out.WriteLine($"Run out ID : {Thread.CurrentThread.ManagedThreadId}");
                Task.Run(() =>
                {
                    Thread.Sleep(1000);
                    Console.Out.WriteLine($"Run in ID : {Thread.CurrentThread.ManagedThreadId}");
                }, token);
                token.Register(
                    () =>
                    {
                        Console.Out.WriteLine($"In Register1 ID : {Thread.CurrentThread.ManagedThreadId}");
                    }, false);
                token.Register(
                    () =>
                    {
                        Console.Out.WriteLine($"In Register2 ID : {Thread.CurrentThread.ManagedThreadId}");
                    }, false);
                token.Register(
                    () =>
                    {
                        Console.Out.WriteLine($"In Register3 ID : {Thread.CurrentThread.ManagedThreadId}");
                    }, false);

                Thread.Sleep(1000);
                cts.Cancel();
            }));
            task.Wait();
        }
    }

    internal class ThreadPoolSynchronizationContext : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object state)
        {
            ThreadPool.QueueUserWorkItem(_ => d(state));
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            d(state);
        }
    }
}