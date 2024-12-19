using System;
using System.Runtime.CompilerServices;

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
            // 屏蔽cmd 的 Ctrl+C 退出
            // Console.CancelKeyPress += (sender, eventArgs) =>
            // {
            //     eventArgs.Cancel = true;
            //     Console.WriteLine("Ctrl+C is disabled");
            // };

            Class c = new Class();
            c = null;
            // try
            // {
            //     Console.ReadLine();
            // }
            // finally
            // {
            //     Console.WriteLine("1");
            // }

            while (true)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            }
        }
    }
}