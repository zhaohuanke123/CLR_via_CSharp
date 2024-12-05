// 测试IL代码里的.maxstack 8 的作用

using System;
using System.Runtime.CompilerServices;

namespace Test.maxstack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Test(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            TestNoNewLocalVar();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void Test(int i, int j, int k, int l, int m, int n, int o, int p, int q, int r, int s, int t, int u,
            int v, int w)
        {
            int a = 1;
            int b = 2;
            int c = 3;
            Console.WriteLine(a + b + c + j + k);
        }

        static void TestNoNewLocalVar()
        {
            TestS(@"{0}{1}{2}{3}", "123", "123", "123", "123", "123", "123", "123", "123", "123");
        }

        static void TestS(string a, string b, string c, string d, string e, string f, string g, string h, string i,
            string l)
        {
            Console.WriteLine(a + b + c + d + f);
        }
    }

}