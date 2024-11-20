using System;

namespace TestMetaTable
{
    internal class Program
    {
        public bool i;
        public int a;
        public int b;
        public char c;
        public byte d;
        public long e;
        public short f;
        public float g;
        public double h;
        public string j;
        public object k;

        public override string ToString()
        {
            return a + " " + b + " " + c + " " + d + " " + e + " " + f + " " + g + " " + h + " " + i + " " + j + " " +
                   k;
        }

        public static void Main(string[] args)
        {
            Program p = new Program();
            p.a = 1;
            p.b = 2;
            p.c = '3';
            p.d = 4;
            p.e = 5;
            p.f = 6;
            p.g = 7;
            p.h = 8;
            p.i = true;
            p.j = "10";
            p.k = new object();
            Console.WriteLine(p);
        }
    }
}