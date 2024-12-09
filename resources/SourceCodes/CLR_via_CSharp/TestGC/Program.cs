using System;
using System.Threading;

namespace TestGC
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Timer t = new Timer(TimerCallback, null, 0, 2000);

            Console.ReadLine();

            t.Dispose();
        }

        private static void TimerCallback(object state)
        {
            Console.WriteLine("in TimerCallbak:" + DateTime.Now);

            GC.Collect();
        }
    }
}