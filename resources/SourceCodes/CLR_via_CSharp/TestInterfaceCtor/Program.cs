using System;
using System.Runtime.CompilerServices;

namespace TestInterfaceCtor
{
    internal interface IF
    {
    }

    struct MyStruct : IF
    {
        public static bool operator ==(MyStruct ms, MyStruct ms2)
        {
            return true;
        }

        public static bool operator !=(MyStruct ms, MyStruct ms2)
        {
            return !(ms == ms2);
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            // 测试实例化 接口 IF
            MyStruct f = new MyStruct();
            MyStruct f2 = new MyStruct();
        }
    }
}