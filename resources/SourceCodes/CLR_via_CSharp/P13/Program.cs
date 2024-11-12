using System;
using System.Reflection.Emit;

namespace P13
{
    interface ITest
    {
        void Test();
    }

    class A : ITest
    {
        void ITest.Test()
        {
            Console.WriteLine("A.Test()");
        }

        public virtual void TestA()
        {
            Console.WriteLine("A.TestA()");
        }
    }

    class B : A
    {
        public void Test()
        {
            Console.WriteLine("B.Test()");
        }

        public sealed override void TestA()
        {
            Console.WriteLine("B.TestA()");
        }
    }

    internal class Program
    {
        static void TestStructInterface<T>(T t) where T : IComparable
        {
            Console.WriteLine(t.CompareTo(t));
        }

        public static void Main(string[] args)
        {
            // SomeValueType.Run();
            Derived.Run();
        }
    }
}