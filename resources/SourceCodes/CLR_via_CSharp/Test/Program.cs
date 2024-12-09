using System;
using System.Collections.Generic;

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
        public static void Main()
        {
        }
    }
}