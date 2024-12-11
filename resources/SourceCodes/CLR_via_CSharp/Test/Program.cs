using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

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
            try
            {
                Thread.CurrentThread.Abort();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                int a = 10;
                int b = 12;
                Console.WriteLine(a + b);
            }
        }
    }
}