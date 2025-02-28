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
                try
                {
                    var program1 = new Program();
                    Thread.Sleep(2000);
                    Console.WriteLine(program1);
                }
                finally
                {
                    Console.WriteLine("Finally");
                }
            }), null);

            var program = new Program();
            Console.WriteLine(program);
        }
    }
}