using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestTaskExcetion
{
    class Program
    {
        static void Main(string[] args)
        {
            // 设置未观察到的异常事件处理
            TaskScheduler.UnobservedTaskException += (sender, eventArgs) =>
            {
                Console.WriteLine($"Unobserved exception caught: {eventArgs.Exception}");
                eventArgs.SetObserved(); // 标记为已观察，防止程序崩溃
            };

            // 创建并启动一个Task
            Task.Run(() =>
            {
                // Thread.Sleep(1000);
                // try
                {
                    throw new InvalidOperationException("This is a test exception");
                }
                // catch (InvalidOperationException e)
                {
                    // Console.WriteLine(e);
                }
            });

            // 强制垃圾回收，触发未观察的异常
            Console.WriteLine("Forcing garbage collection...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // 给时间让异常处理触发
            Thread.Sleep(1000);

            Console.WriteLine("Main method complete");
        }
    }
}