using System;
using System.Diagnostics;

namespace TestBeforeFieldInit
{
    class TestCtor
    {
        public TestCtor()
        {
            // Console.WriteLine($"TestCtor {name}");
            // 获取当前调用堆栈的信息
            var stackTrace = new StackTrace();
            var typeName = stackTrace.GetFrame(1).GetMethod().DeclaringType.Name;

            Console.WriteLine("当前类: " + typeName);
        }
    }

    internal class BeforeInit
    {
        public static TestCtor testCtor = new TestCtor();
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

    internal class Program
    {
        public static void Main(string[] args)
        {
            _ = NotBefore.testCtor;
            Console.WriteLine("------------");
            _ = BeforeInit.testCtor;
            Console.WriteLine("------------");
            _ = Class1.testCtor;
            Console.WriteLine("------------");
        }
    }
}