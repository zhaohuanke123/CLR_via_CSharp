using System;

namespace TestInvokationList
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Action a = () => { };
            Action b = null;
            b += a;
            b += a;
            b += a;

            foreach (var dele in b.GetInvocationList())
            {
                Console.WriteLine(ReferenceEquals(dele, a));
            }
        }
    }
}