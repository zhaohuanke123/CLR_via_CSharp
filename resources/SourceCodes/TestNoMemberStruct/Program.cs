using System;

namespace TestNoMemberStruct
{
    struct MyStruct
    {
        // 没有成员
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            MyStruct ms = new MyStruct();
            Console.WriteLine(ms.ToString());
        }
    }
}
