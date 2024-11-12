using System;

namespace Test
{
    public class TestStructctor
    {
        struct A
        {
            public static int a;

            public static void Test()
            {
                Console.WriteLine("A.Test");
            }

            static A()
            {
                Console.WriteLine("A static ctor");
                a = 10;
            }

            public int t;

            public A(int a)
            {
                t = 1;
                Console.WriteLine("A ctor");
            }

            public void TestNotStatic()
            {
                Console.WriteLine("A.TestNotStatic");
            }
        }

        class B
        {
            static B()
            {
                Console.WriteLine("B static ctor");
            }

            public static void Test()
            {
                Console.WriteLine("B.Test");
            }
        }

        public static void Run()
        {
            Console.WriteLine("Before A------------------");
            A a = new A();
            object o = a;
            A t = (A)o;
            // a.TestNotStatic();
            // Console.WriteLine(A.a);
            // A a = new A();
            // a.t = 2;
            // Console.WriteLine(a.t);
            // A.Test();
            Console.WriteLine("After A------------------");

            Console.WriteLine("Before B ------------------");
            // B b = new B();
            // B.Test();
            Console.WriteLine("After B------------------");
        }
    }
}