using System;

namespace TestLock
{
    internal class Program
    {
        private static Program p = new Program();
        private static Program p1 = new Program();
        private static Program p2 = new Program();
        private static Program p3 = new Program();

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            lock (p)
            {
                lock (p1)
                {
                    lock (p2)
                    {
                        Console.WriteLine("Hello World!");
                    }
                }
            }
        }
    }
}