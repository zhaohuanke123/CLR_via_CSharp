using System;
using System.Reflection;
using System.Threading.Tasks;

namespace TestParallel
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Parallel.Invoke(
                Test,
                Test,
                Test,
                Test
            );

            Console.ReadLine();
        }

        static async void Test()
        {
            Console.WriteLine(123);
            Console.WriteLine();
            await Task.Delay(1000);
            Console.WriteLine(123);
        }
    }
}