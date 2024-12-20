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
            Thread.Sleep(1000);
            var list = new List<A>();
            for (long x = 0; x < 1000; ++x)
            {
                list.Add(new A()
                {
                    padding = GC.AllocateArray<byte>(100, true),
                });
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Before CC");
            try
            {
                Console.WriteLine("Press Ctrl+C to exit");
                Console.ReadLine();
            }
            finally
            {
                Console.WriteLine("finally");
            }

            Console.WriteLine("After CC");

            // var threads = new List<Thread>();
            // for (var x = 0; x < 100; ++x)
            // {
            //     var thread = new Thread(ThreadBody);
            //     threads.Add(thread);
            //     thread.Start();
            // }
            //
            // Console.WriteLine(DateTime.Now);
            // foreach (var thread in threads)
            // {
            //     thread.Join();
            // }
            //
            // Console.WriteLine(DateTime.Now);
            // GC.Collect();
            // Console.WriteLine(DateTime.Now);
            // Console.WriteLine("memory released");
            //
            // using (var file = new StreamWriter("log.txt"))
            // {
            //     while (queue.TryDequeue(out var line))
            //     {
            //         file.Write(line);
            //     }
            // }
            //
            // Console.WriteLine(DateTime.Now);
            // Console.WriteLine("log written");
            // Console.ReadLine();
        }
    }
}