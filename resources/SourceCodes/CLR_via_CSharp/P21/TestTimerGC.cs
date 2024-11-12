using System;
using System.Threading;

namespace P21
{
    public class TestTimerGC
    {
        public static void Run()
        {
            Timer t = new Timer(TimerCallback, null, 0, 2000);

            Console.ReadLine();

            // t.Dispose();
        }

        private static void TimerCallback(object o)
        {
            Console.WriteLine("in timerCallback:" + DateTime.Now);

            GC.Collect();
        }
    }
}