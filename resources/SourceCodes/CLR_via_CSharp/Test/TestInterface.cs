namespace Test
{
    using System;

    internal class Base : IComparable
    {
        // 显式接口方法实现(EIMI)
        Int32 IComparable.CompareTo(Object o)
        {
            Console.WriteLine("Base's IComparable.CompareTo");
            return CompareTo(o); // 调用虚方法
        }

        // 用于派生类的虚方法(该方法可为任意名称)
        public virtual Int32 CompareTo(Object o)
        {
            Console.WriteLine("Base's virtual CompareTo");

            return 0;
        }
    }

    internal sealed class Derived : Base, IComparable
    {
        // 一个公共方法，也是接口的实现
        public override Int32 CompareTo(Object o)
        {
            Console.WriteLine("Derived's CompareTo");

            // 现在可以调用 Base 的虚方法
            return base.CompareTo(o);
        }
    }

    class Test
    {
        public static void Go()
        {
            Derived derived1 = new Derived();
            Derived derived2 = new Derived();
            //derived1.CompareTo(derived2);
            IComparable iComparable = derived1;
            iComparable.CompareTo(derived2);
        }
    }
}