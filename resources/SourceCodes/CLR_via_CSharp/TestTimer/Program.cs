using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestTimer
{
    internal class Program
    {
        private static Timer s_timer;

        public static void Main()
        {
            Console.WriteLine("Checking status every 2 seconds");

            // 创建但不启动计时器。确保 s_timer 在线程池线程调用 Status 之前引用该计时器
            s_timer = new Timer(Status, null, Timeout.Infinite, Timeout.Infinite);

            // 现在 s_timer 已被赋值，可以启动计时器了
            // 现在在 Status 中调用 Change，保证不会抛出 NullReferenceException
            s_timer.Change(0, Timeout.Infinite);

            Console.ReadLine(); // 防止进程终止
        }

        // 这个方法的签名必须和 TimerCallback 委托匹配
        private static void Status(Object state)
        {
            // 这个方法由一个线程池线程执行
            Console.WriteLine("In Status at {0}", DateTime.Now);
            // Thread.Sleep(1000); // 模拟其他工作(1 秒)
            Task.Delay(1000);

            // 返回前让 Timer 在 2 秒后再次触发
            s_timer.Change(2000, Timeout.Infinite);

            // 这个方法返回后，线程回归池中，等待下一个工作项
        }
    }
}