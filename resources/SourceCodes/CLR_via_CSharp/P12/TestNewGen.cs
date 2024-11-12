using System;
using System.Diagnostics;

namespace P12
{
    public class TestNewGen
    {
        const int N = 1000000;

        public static void Run()
        {
            TestNewGen t = new TestNewGen();

            // 测试 Test 方法和 TestNoGen 方法的性能差异

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < N; i++)
            {
                t.Test(1);
            }

            sw.Stop();
            Console.WriteLine("Test: " + sw.ElapsedMilliseconds);

            sw.Restart();
            for (int i = 0; i < N; i++)
            {
                t.TestNoGen(1);
            }

            sw.Stop();
            Console.WriteLine("TestNoGen: " + sw.ElapsedMilliseconds);

            sw.Restart();
            for (int i = 0; i < N; i++)
            {
                t.TestInterface(1);
            }

            sw.Stop();
            Console.WriteLine("TestInterface: " + sw.ElapsedMilliseconds);

            sw.Restart();
            for (int i = 0; i < N; i++)
            {
                t.TestInterfaceNoGen(1);
            }

            sw.Stop();
            Console.WriteLine("TestInterfaceNoGen: " + sw.ElapsedMilliseconds);
        }

        public void Test<T>(T t) where T : struct
        {
            T t2 = new T();
        }

        public void TestInterface<T>(T t) where T : IComparable<T>
        {
            var compareTo = t.CompareTo(t);
            // 提示编译器不要优化
            if (compareTo == 1)
            {
                Console.WriteLine("compareTo == 0");
            }
        }

        public void TestNoGen(int i)
        {
            int i2 = new int();
        }

        public void TestInterfaceNoGen(int i)
        {
            var compareTo = i.CompareTo(10);
            // 提示编译器不要优化
            if (compareTo == 1)
            {
                Console.WriteLine("compareTo == 0");
            }
        }
    }
}