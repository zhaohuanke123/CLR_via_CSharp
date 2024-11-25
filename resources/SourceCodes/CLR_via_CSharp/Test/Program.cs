using System;
using System.Runtime.Serialization;

namespace Test
{
    internal class EquClass
    {
        private int a;
        private int b;
        public string s;

        public EquClass(int a, int b, string s)
        {
            this.a = a;
            this.b = b;
            this.s = s;
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            EquClass a = new EquClass(1, 2, "hhhh");
            var hash = a.GetHashCode();
            lock (a)
            {
                lock (a)
                {
                    Console.WriteLine(hash);
                }
            }
        }
    }
}