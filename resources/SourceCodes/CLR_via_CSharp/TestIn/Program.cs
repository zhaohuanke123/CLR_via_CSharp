using System;

namespace TestIn
{
    internal class Program
    {
        struct MyStruct
        {
            public int a;
            public int b;

            public void Increment()
            {
                a++;
                b++;
            }

            public override string ToString()
            {
                return $"a: {a.ToString()}, b: {b.ToString()}";
            }
        }

        public static void Main(string[] args)
        {
            var a = new MyStruct();
            a.Increment();
            Test(a);
            Console.WriteLine(a);
        }

        static void Test(in MyStruct a)
        {
            a.Increment(); // 发生防御性拷贝
        }
    }
}