namespace TestEventDelegate
{
    class Program
    {
        // 定义一个委托类型
        public delegate void MyDelegate(string message);

        // 使用委托
        public static MyDelegate MyDelegateInstance;

        // 使用事件
        public static event MyDelegate MyEvent;

        static void Main(string[] args)
        {
            // 设置测试方法，传入多线程
            TestDelegate();
            TestEvent();
        }

        // 测试委托
        static void TestDelegate()
        {
            Console.WriteLine("Testing Delegate with Multiple Threads:");

            // 线程数量
            int threadCount = 5;
            Task[] tasks = new Task[threadCount];

            // 订阅委托

            // 启动多个线程，模拟并发调用委托
            for (int i = 0; i < threadCount; i++)
            {
                int threadIndex = i; // 捕获当前线程索引
                tasks[i] = Task.Run(() =>
                {
                    // 每个线程触发委托
                    MyDelegateInstance += MessageHandler;
                });
            }

            // 等待所有线程执行完
            Task.WhenAll(tasks).Wait();
            MyDelegateInstance?.Invoke($"Thread calling delegate.");
            Console.WriteLine();
        }

        // 测试事件
        static void TestEvent()
        {
            Console.WriteLine("Testing Event with Multiple Threads:");

            // 线程数量
            int threadCount = 5;
            Task[] tasks = new Task[threadCount];

            // 订阅事件

            // 启动多个线程，模拟并发调用事件
            for (int i = 0; i < threadCount; i++)
            {
                int threadIndex = i; // 捕获当前线程索引
                tasks[i] = Task.Run(() =>
                {
                    MyEvent += MessageHandler;
                    // 每个线程触发事件
                });
            }

            // 等待所有线程执行完
            Task.WhenAll(tasks).Wait();
            MyEvent?.Invoke($"Thread calling event.");
            Console.WriteLine();
        }

        // 事件和委托的处理程序
        static void MessageHandler(string message)
        {
            Console.WriteLine($"Handler received: {message}");
        }
    }
}