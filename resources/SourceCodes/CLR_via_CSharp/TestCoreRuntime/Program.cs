using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestCoreRuntime
{
    public class A
    {
        public int a;
        public byte[] padding;

        ~A()
        {
            Program.queue.Enqueue("A" + DateTime.Now + "\n");
        }
    }

    public class Program
    {
        public static ConcurrentQueue<string> queue = new ConcurrentQueue<string>();

        public static void ThreadBody()
        {
            Console.WriteLine("ThreadBody Started");
            Thread.Sleep(1000);
            Console.WriteLine("ThreadBody Ended");
        }

        public static void Main(string[] args)
        {
            Thread thread = new Thread(new ThreadStart(ThreadBody));
            thread.Start();
            try
            {
                // Console.WriteLine("Press Ctrl+C to exit");
                // Console.ReadLine();
            }
            finally
            {
                Console.WriteLine("finally");
            }

            Console.WriteLine("After CC");
        }
    }
}