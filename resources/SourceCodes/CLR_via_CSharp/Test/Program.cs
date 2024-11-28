using System;
using System.Runtime.CompilerServices;

namespace Test
{
    internal static class Program
    {
        class MyClass
        {
            public string s;
        }

        public static void Main(string[] args)
        {
            MyClass ms = new MyClass();
            ms.s = "123123";
            Test(ref ms);
            Console.WriteLine(ms.s);
        }

        static void Test(ref MyClass ms)
        {
            ms.s = "123";
        }

        static void Test(string s)
        {
            s = "123";
        }
    }
}