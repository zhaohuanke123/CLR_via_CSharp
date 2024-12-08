using System;

namespace C20TestException
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Finally");
                throw new Exception();
            }
        }
    }
}