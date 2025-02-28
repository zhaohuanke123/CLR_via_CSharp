namespace Test1
{
    internal class Program
    {
        public static readonly object o = new object();
        public static int i = 0;

        private static void Main(string[] args)
        {
            // new Thread(Third).Start(() =>
            // {
            //     Monitor.Enter(o);
            //
            //     while (i != 2)
            //     {
            //         Monitor.Wait(o);
            //     }
            //     Console.WriteLine("third");
            //
            //     i = 3;
            //     Monitor.PulseAll(o);
            //     Monitor.Exit(o);
            // });
            // new Thread(Second).Start(() =>
            // {
            //     Monitor.Enter(o);
            //
            //     while (i != 1)
            //     {
            //         Monitor.Wait(o);
            //     }
            //
            //     Console.WriteLine("second");
            //
            //     i = 2;
            //     Monitor.PulseAll(o);
            //     Monitor.Exit(o);
            // });
            // new Thread(First).Start(() =>
            // {
            //     Monitor.Enter(o);
            //     Console.WriteLine("first");
            //     i = 1;
            //     Monitor.PulseAll(o);
            //     Monitor.Exit(o);
            // });

            // ManualResetEvent mre1 = new ManualResetEvent(false);
            // ManualResetEvent mre2 = new ManualResetEvent(false);
            // new Thread(Third).Start(() =>
            // {
            //     mre2.WaitOne(); 
            //     Console.WriteLine("third");   
            // });
            // new Thread(Second).Start(() =>
            // {
            //     mre1.WaitOne();
            //     Console.WriteLine("second");
            //     mre2.Set();
            // });
            // new Thread(First).Start(() =>
            // {
            //     Console.WriteLine("first");
            //     mre1.Set();
            // });

            new Thread(Third).Start(() =>
            {
                while (Interlocked.CompareExchange(ref i, 5, 4) != 4)
                {
                }
                Console.WriteLine("third");
            });
            new Thread(Second).Start(() =>
            {
                while (Interlocked.CompareExchange(ref i, 3, 2) != 2)
                {
                    
                }
                Console.WriteLine("second");
                Volatile.Write(ref i, 4);
            });
            new Thread(First).Start(() =>
            {
                while (Interlocked.CompareExchange(ref i, 1, 0) != 0)
                {
                    
                }
                Console.WriteLine("first");
                Volatile.Write(ref i, 2);
            });
        }

        public static void First(object? first)
        {
            ((Action)first)();
        }

        public static void Second(object? second)
        {
            ((Action)second)();
        }

        public static void Third(object? third)
        {
            ((Action)third)();
        }
    }
}