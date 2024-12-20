namespace TestMutex
{
    internal static class Program
    {
        private static void Main()
        {
            bool createdNew;
            using (Mutex mutex = new Mutex(false, "Global\\MyNamedMutex", out createdNew))
            {
                if (!createdNew)
                {
                    Console.WriteLine("Mutex 已存在，可能已有另一个实例在运行。");
                }

                // 请求获取 Mutex
                mutex.WaitOne();

                try
                {
                    Console.WriteLine("Mutex 已获取");
                    // 执行需要同步的操作
                    Console.ReadLine();
                }
                finally
                {
                    // 释放 Mutex
                    Console.WriteLine("释放 Mutex");
                    mutex.ReleaseMutex();
                }
            }
        }
    }
}