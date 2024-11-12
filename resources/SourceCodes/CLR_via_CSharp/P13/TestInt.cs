using System;

namespace P13
{
    public class TestInt
    {
        public static void Run()
        {
            int x = 5;
            var s = ((IConvertible)x).ToSingle(null);
            Console.WriteLine(s);
        }
    }
}