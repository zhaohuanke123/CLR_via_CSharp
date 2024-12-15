using System;
using System.Diagnostics;

namespace TestNameAndFullName
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var type = typeof(Program);
            Console.WriteLine(type.Name);
            Console.WriteLine(type.FullName);
        }
    }
}