using System;

namespace TestThrowIsSame
{
    internal class Program
    {
        private static Exception e1;
        private static Exception e2;

        public static void Main(string[] args)
        {
            try
            {
                Test();
            }
            catch (Exception e)
            {
                Console.WriteLine(ReferenceEquals(e1, e));
            }
        }

        public static void Test()
        {
            try
            {
                throw new Exception();
            }
            catch (Exception e)
            {
                e1 = e;
                throw;
            }
        }
    }
}