using System;

namespace TestStringPool
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string s = "hehe";
            string s1 = "hehehe";
            string s2 = "hehehehe";

            lock (s)
            {
                Console.WriteLine(s + s1 + s2);
            }
        }
    }
}