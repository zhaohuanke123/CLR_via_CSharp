using System;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;

namespace TestAwait
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            SynchronizationContext.SetSynchronizationContext(new ThreadPoolSynchronizationContext());
            CallContext.LogicalSetData("AAA", 1);
            Run().Wait();
        }

        public static async Task Run()
        {
            // ExecutionContext.SuppressFlow();
            Console.Out.WriteLine($"ID : {Thread.CurrentThread.ManagedThreadId}");
            await Run2();
            Console.Out.WriteLine($"ID : {Thread.CurrentThread.ManagedThreadId}");
        }

        public static Task Run2()
        {
            var task = Task.Run(() =>
            {
                Task.Delay(100).Wait();
                throw new Exception();
                // Console.Out.WriteLine($"ID Run2 : {Thread.CurrentThread.ManagedThreadId}");
            });
            return task;
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