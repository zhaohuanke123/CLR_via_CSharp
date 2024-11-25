using System;

namespace TestTypeObject
{
    internal class Class1
    {
        public static int x;

        static Class1()
        {
            Console.WriteLine("Class1 static constructor");
        }
    }

    internal class Class2
    {
        public static int x;

        static Class2()
        {
            Console.WriteLine("Class2 static constructor");
        }
    }

    struct Struct1
    {
        static Struct1()
        {
            Console.WriteLine($"{nameof(Struct1)} static constructor");
        }

        public Struct1(int a)
        {
        }

        public override string ToString()
        {
            return "Struct1";
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            //TestNew();
            // TestStaticField();
            TestStructHasParameterizedConstructor();
        }

        static void TestNew()
        {
            Class1 c1 = new Class1();
            Console.WriteLine("-------------" + c1);
            Class2 c2 = new Class2();
            Console.WriteLine("-------------" + c2);
        }

        static void TestStaticField()
        {
            Class1.x = 1;
            Console.WriteLine("-------------");
            Class2.x = 2;
            Console.WriteLine("-------------");
        }

        static void TestStruct()
        {
            Struct1 s1 = new Struct1();
            Console.WriteLine("-------------" + s1);
        }
        
        static void TestStructHasParameterizedConstructor()
        {
            Struct1 s1 = new Struct1(1);
            Console.WriteLine("-------------" + s1);
        }
    }
}