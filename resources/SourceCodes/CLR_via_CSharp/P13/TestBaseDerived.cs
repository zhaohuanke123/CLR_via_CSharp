using System;

namespace P13
{
    internal class Base : IComparable
    {
        // 显式接口方法实现
        Int32 IComparable.CompareTo(Object o)
        {
            Console.WriteLine("Base's CompareTo");
            return 0;
        }
    }


    internal sealed class Derived : Base, IComparable
    {
        public static void Run()
        {
            Derived d = new Derived();
            d.CompareTo(null);
        }
        // 一个公共方法，也是接口的实现
        public Int32 CompareTo(Object o)
        {
            Console.WriteLine("Derived's CompareTo");

            // 试图调用基类的 EIMI 导致编译错误：
            // error CS0117：“Base” 不包含 “CompareTo” 的定义
            // base.CompareTo(o);
            IComparable c = this;
            c.CompareTo(o);
            return 0;
        }
    }
}