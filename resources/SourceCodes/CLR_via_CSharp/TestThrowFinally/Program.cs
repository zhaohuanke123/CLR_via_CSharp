using System;

namespace TestThrowFinally
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                throw new Exception();
            }
            finally
            {
                Console.WriteLine(123);
            }
        }
    }
}