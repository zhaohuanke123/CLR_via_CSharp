using System;
using System.Collections.Generic;

namespace Lambda
{
    internal class Program
    {
        private static List<Func<int, int, int>> l;

        public static void Main(string[] args)
        {
            Func<int, int, int> ac1 = (a, b) => { return a + b; };
            l = new List<Func<int, int, int>>();
            l.Add(ac1);
            if (CheckEqual())
            {
                Console.WriteLine("Equal");
            }
        }

        static bool CheckEqual()
        {
            for (int i = 0; i < l.Count - 1; i++)
            {
                if (!ReferenceEquals(l[i], l[i + 1]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}