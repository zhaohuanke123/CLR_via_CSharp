using System;
using System.Collections.Generic;

namespace P12
{
    delegate T2 TestDelegate<in T1, out T2>(T1 t1);

    public class TestInOut
    {
        static TestDelegate<List<int>, object> d;

        public static void Run()
        {
            TestDelegate<IList<int>, string> dd = Test;
            d = Test;

            List<int> s = new List<int> { 1, 2, 3 };
            var o1 = d(s);
            Console.WriteLine(o1);
        }

        public static string Test(IList<int> o)
        {
            Console.WriteLine(o);
            string t = "456";
            return t;
        }
    }
}