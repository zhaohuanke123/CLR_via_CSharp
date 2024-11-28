using System;

namespace TestStaticCtor
{
    internal static class Program
    {
        static Program()
        {
            Console.WriteLine(nameof(Program) + "CCtor");
        }

        static void Main(string[] args)
        {
            Console.WriteLine(nameof(Main));
        }
    }
}
