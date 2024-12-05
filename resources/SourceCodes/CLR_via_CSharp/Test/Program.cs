using System;

namespace Test
{
    interface IInterface
    {
        void Test();
    }

    class TestClass : IInterface
    {
        public void Test()
        {
            Console.WriteLine("Test");
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            TestClass test = new TestClass();
            IInterface test2 = test;
            test.Test();
            test2.Test();
        }
    }
}