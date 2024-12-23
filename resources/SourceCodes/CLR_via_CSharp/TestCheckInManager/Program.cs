using System.Threading;

namespace TestCheckInManager
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Thread[] threads = new Thread[10];
            for (int i = 0; i < 10; i++)
            {
                threads[i] = new Thread(() =>
                {
                    while (true) ;
                });
                threads[i].Start();
            }

            for (int i = 0; i < 10; i++)
            {
                threads[i].Join();
            }
        }
    }
}