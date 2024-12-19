using System.Collections.Concurrent;
using System.Runtime.InteropServices;

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
            var s = $"asd{1}asd";
            Console.WriteLine(s);
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