using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestTaskParent
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // var parent = Task.Factory.StartNew(() =>
            // {
            //     var child = Task.Factory.StartNew(() => { Thread.Sleep(1000); });
            // });
            //
            // Console.WriteLine("Start");
            // parent.Wait();
            // Console.WriteLine("Press any key to continue...");
            Test().Wait();
        }

        public static async Task Test()
        {
            var d1 = Task.Delay(1000);
            var d2 = Task.Delay(500);

            await d1;
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            await d2;
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            await Task.Delay(100);
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        }
    }
}