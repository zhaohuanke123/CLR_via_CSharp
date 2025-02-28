using System.Collections.Concurrent;

namespace TestSynchronizationContext
{
    internal class Program
    {
        public static ConcurrentQueue<Action> actions = new ConcurrentQueue<Action>();

        public static void Main(string[] args)
        {
            SynchronizationContext.SetSynchronizationContext(new MainThreadSynchronization());
            _ = Test();

            while (true)
            {
                while (actions.TryDequeue(out var action))
                {
                    try
                    {
                        action.Invoke();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        private static async Task Test()
        {
            Console.WriteLine($"ID : {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(50);
            Console.WriteLine($"ID : {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(50);
            Console.WriteLine($"ID : {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(50);
            Console.WriteLine($"ID : {Thread.CurrentThread.ManagedThreadId}");
        }
    }
    internal class MainThreadSynchronization : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object? state)
        {
            Program.actions.Enqueue(() => d(state));
        }

    }
}