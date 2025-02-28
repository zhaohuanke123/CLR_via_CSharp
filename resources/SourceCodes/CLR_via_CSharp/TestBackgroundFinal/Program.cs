using System;
using System.Threading;

namespace TestBackgroundFinal
{
    internal class Program
    {
        ~Program()
        {
            Console.WriteLine("Destructor called");
        }

        public static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(((o) =>
            {
                while (true)
                {
                    try
                    {
                        var program1 = new Program();
                        Thread.Sleep(10);
                        // Console.WriteLine(program1);
                    }
                    finally
                    {
                        Console.WriteLine("Finally");
                    }
                }
            }), null);

            Thread.Sleep(100);
            Console.WriteLine("end");
        }
    }
}