using System;

namespace NullableDef
{
    internal class Program
    {
        private const int a = 10;

        public static void Main(string[] args)
        {
            TestSize.Go();
            TestGetType.Go();
            GC.Collect();
        }
    }

    class TestGetType
    {
        public static void Go()
        {
            int? a = 1;
            object o = a;
            var type = o.GetType();
            Console.WriteLine(type);
        }
    }

    class TestSize
    {
        public static void Go()
        {
            unsafe
            {
                Console.WriteLine(sizeof(double?));
            }
        }
    }
}