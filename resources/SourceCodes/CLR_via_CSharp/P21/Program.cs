using System;
using System.Collections.Generic;

namespace P21
{
    struct MyStruct
    {
        public int i;
        public int j;
        public int k;
    }
    internal class Program
    {
        public int i;
        public int j;

        public static void Main(string[] args)
        {
            object o = new Program();
            Console.WriteLine(o);
        }
    }
}