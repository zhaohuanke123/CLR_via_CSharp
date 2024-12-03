using System;
using System.Diagnostics;

namespace TestRefrection
{
    internal class Program
    {
        public Program()
        {
            Console.WriteLine("Hello World!");
        }

        public static void Main(string[] args)
        {
            var instance = Activator.CreateInstance(typeof(Program));
            var program = instance as Program;
        }
    }
}