using System;

namespace Test
{
    internal class Program
    {
        public static void Main()
        {
            string s = "he";
            string s2 = "hehe";
            var type = s.GetType();
            string s2c = type.ToString();
            Console.WriteLine(s2c);
        }
    }
}