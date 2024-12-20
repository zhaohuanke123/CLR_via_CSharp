using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace TestCtrlCExit
{
    class Class
    {
        public Class()
        {
        }

        ~Class()
        {
            Console.WriteLine("Destructor");
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            // bool createdNew;
            // using (Mutex mutex = new Mutex(false, "Global\\MyNamedMutex", out createdNew))
            // {
            //     if (!createdNew)
            //     {
            //         Console.WriteLine("Mutex 已存在，可能已有另一个实例在运行。");
            //     }
            //
            //     // 请求获取 Mutex
            //     mutex.WaitOne();

            // }
            // 屏蔽cmd 的 Ctrl+C 退出
            // Console.CancelKeyPress += (sender, eventArgs) =>
            // {
            //     eventArgs.Cancel = true;
            //     Console.WriteLine("Ctrl+C is disabled");
            // };
            //
            // Class c = new Class();
            // c = null;

            try
            {
                Console.WriteLine("Press Ctrl+C to exit");
                Console.ReadLine();
            }
            finally
            {
                Console.WriteLine("finally");
            }

            // while (true)
            // {
            //     GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            // }
        }
    }
}