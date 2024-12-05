using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace TestBeforeFieldInit
{
    class TestCtor
    {
        public TestCtor()
        {
            // Console.WriteLine($"TestCtor {name}");
            // 获取当前调用堆栈的信息
            var stackTrace = new StackTrace();
            var mb = stackTrace.GetFrame(1).GetMethod();
            var type = mb.DeclaringType;
            var typeName = type.Name;

            // Console.WriteLine("当前类: " + typeName + "   当前线程ID : " + Thread.CurrentThread.ManagedThreadId);
            var s = "当前类: " + typeName + "   当前线程ID : " + Thread.CurrentThread.ManagedThreadId;
            Program.que.Enqueue(s);
        }
    }

    internal class BeforeInit
    {
        public static TestCtor testCtor = new TestCtor();

        static BeforeInit()
        {
            
        }
    }

    internal class Class1
    {
        public static TestCtor testCtor = new TestCtor();
    }

    internal class NotBefore
    {
        public static TestCtor testCtor;

        static NotBefore()
        {
            testCtor = new TestCtor();
        }
    }

    internal partial class Program
    {
        public static ConcurrentQueue<string> que = new ConcurrentQueue<string>();

        public static void Main(string[] args)
        {
            que.Enqueue("StartMain");
            Do();
        }

        public static void Do()
        {
            que.Enqueue("Start");
            _ = NotBefore.testCtor;
            que.Enqueue("-----------------");
            // Console.WriteLine("------------");
            _ = BeforeInit.testCtor;
            que.Enqueue("-----------------");
            // Console.WriteLine("------------");
            _ = Class1.testCtor;
            que.Enqueue("-----------------");
            // Console.WriteLine("------------");

            foreach (var item in que)
            {
                Console.WriteLine(item);
            }
        }
    }
}