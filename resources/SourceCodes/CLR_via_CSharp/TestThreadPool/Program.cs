using System;
using System.Threading;

namespace TestThreadPool
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem((state => { throw new Exception(); }), null);
            Thread.Sleep(1000);
            Console.Out.WriteLine("Press any key to exit...");
        }
    }
}