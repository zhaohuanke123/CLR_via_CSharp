using System;
using System.Reflection;
using System.Threading.Tasks;

namespace TestParallel
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Parallel.Invoke(Test, Test);

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        private static async void Test()
        {
            Console.WriteLine("Starting Test...");
            Console.WriteLine("End Test");
        }
    }
}