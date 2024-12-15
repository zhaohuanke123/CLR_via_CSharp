using System;

namespace TestFix
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            int[] a = { 1, 2, 3, 3 };
            unsafe
            {
                fixed (int* p = a)
                {
                    Console.WriteLine((IntPtr)p);
                }
            }

            Console.WriteLine(123);
            Console.WriteLine(123);
            GC.Collect();
        }
    }
}