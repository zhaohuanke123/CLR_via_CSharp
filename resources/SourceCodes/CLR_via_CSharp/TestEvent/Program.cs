using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestEvent
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Test().Wait();
            return;
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            autoEvent.Set();
            // Thread.Sleep(500);

            var task = Task.Run(() =>
            {
                autoEvent.WaitOne();
                Console.WriteLine("waited event");
            });
            var task2 = Task.Run(() =>
            {
                autoEvent.WaitOne();
                Console.WriteLine("waited event");
            });

            Console.WriteLine("Wait for task");
            task.Wait();
            task2.Wait();
        }

        public static async Task Test()
        {
            Console.WriteLine("Test started");
            
            try
            {
                await Task.Run(() => { throw new Exception(); });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Test end");
        }
    }
}