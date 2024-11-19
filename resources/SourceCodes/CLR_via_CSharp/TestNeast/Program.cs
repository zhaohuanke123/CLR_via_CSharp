using System;

namespace TestNeast
{
    internal class Program
    {
        private static int a;

        class Test
        {
            public void Test1()
            {
                Console.WriteLine("Test1: " + a);
            }
        }

        public static void Main()
        {
            Test t = new Test();
            t.Test1();
        }
    }
}