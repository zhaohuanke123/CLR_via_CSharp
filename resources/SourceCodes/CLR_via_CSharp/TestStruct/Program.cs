using System;
using System.Collections.Generic;

namespace TestStruct
{
    internal class Program
    {
        struct MyStruct
        {
            public int a;

            public void Increment()
            {
                a++;
            }

            public MyStruct(int a)
            {
                this.a = a;
            }
        }

        static void Main()
        {
            List<MyStruct> list = new List<MyStruct>();
            list.Add(new MyStruct(10));
            Console.WriteLine(list[0].a);
        }
    }
}