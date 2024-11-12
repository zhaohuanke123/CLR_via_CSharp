using System;

namespace P13
{
    internal struct SomeValueType : IComparable
    {
        public static void Run()
        {
            //  int i = 10;
            // TestStructInterface(i);
            SomeValueType v = new SomeValueType(0);
            IComparable c = v; // 装箱！

            Object o = new Object();
            Int32 n = c.CompareTo(v); // 不希望的装箱操作
            n = c.CompareTo(o); // InvalidCastException 异常 
        }

        private Int32 m_x;

        public SomeValueType(Int32 x)
        {
            m_x = x;
        }

        public Int32 CompareTo(SomeValueType other)
        {
            return (m_x - other.m_x);
        }

        // 注意以下代码没有指定 pulbic/private 可访问性
        Int32 IComparable.CompareTo(Object other)
        {
            return CompareTo((SomeValueType)other);
        }
    }
}