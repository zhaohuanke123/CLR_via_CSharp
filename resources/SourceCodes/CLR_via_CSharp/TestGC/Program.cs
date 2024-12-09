using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;

namespace TestGC
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // TestTimer.Go();
            TestGCHandle.Go(null);
        }
    }

    // 测试的对象类
    class TestObject
    {
        public string Name { get; set; }

        // 析构函数，用于模拟资源释放
        ~TestObject()
        {
            TestGCHandle.outputQueue.Enqueue("TestObject finalized");
        }
    }

    class TestGCHandle
    {
        public static ConcurrentQueue<string> outputQueue = new ConcurrentQueue<string>();

        public static void Go(string[] args)
        {
            // 创建一个 ConcurrentQueue 来收集输出

            // 创建一个 TestObject 对象
            TestObject obj = new TestObject { Name = "Test Object" };

            // 使用GCHandle的Weak引用
            GCHandle weakHandle = GCHandle.Alloc(obj, GCHandleType.Weak);
            // 使用GCHandle的WeakTrackResurrection引用
            GCHandle weakTrackResHandle = GCHandle.Alloc(obj, GCHandleType.WeakTrackResurrection);

            // 通过弱引用访问对象
            outputQueue.Enqueue("Weak reference: " +
                                (weakHandle.Target != null ? "Object is alive" : "Object is collected"));
            outputQueue.Enqueue("WeakTrackResurrection reference: " +
                                (weakTrackResHandle.Target != null ? "Object is alive" : "Object is collected"));

            // 使对象不可访问（让其成为垃圾回收的候选对象）
            obj = null;

            // 强制进行垃圾回收
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // 检查弱引用是否有效
            outputQueue.Enqueue("\nAfter GC Collect:");

            // 在 GCHandle 为 Weak 的情况下，应该无法再访问对象
            outputQueue.Enqueue("Weak reference: " +
                                (weakHandle.Target != null ? "Object is alive" : "Object is collected"));

            GC.Collect(2, GCCollectionMode.Forced);

            // 在 GCHandle 为 WeakTrackResurrection 的情况下，虽然对象已经被回收，但因为启用了复生追踪，仍然能访问到对象
            outputQueue.Enqueue("WeakTrackResurrection reference: " +
                                (weakTrackResHandle.Target != null ? "Object is alive" : "Object is collected"));

            // 统一输出
            Console.WriteLine("\nTest Results:");
            while (outputQueue.TryDequeue(out string result))
            {
                Console.WriteLine(result);
            }
        }
    }

    class TestTimer
    {
        public static void Go()
        {
            Timer t = new Timer(TimerCallback, null, 0, 2000);

            Console.ReadLine();

            t.Dispose();
        }

        private static void TimerCallback(object state)
        {
            Console.WriteLine("in TimerCallbak:" + DateTime.Now);

            GC.Collect();
        }
    }
}