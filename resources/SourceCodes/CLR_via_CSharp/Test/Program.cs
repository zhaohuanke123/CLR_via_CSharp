using System;

namespace Test
{
    internal class Program
    {
        static Program()
        {
            Console.WriteLine("static constructor " + nameof(Program));
        }

        public static void Main()
        {
            Console.WriteLine("Hello, World!");
        }
    }
}