//  测试调用值类型的类型构造器

using System.Runtime.InteropServices.ComTypes;

namespace TestStructCCtor
{
    using System;

    public class Program
    {
        struct MyStruct
        {
            static MyStruct()
            {
                Console.WriteLine("My struct ctor");
            }

            public MyStruct(int i)
            {
            }
        }

        public static void Main(string[] args)
        {
            MyStruct ms = new MyStruct(1);
        }
    }
}