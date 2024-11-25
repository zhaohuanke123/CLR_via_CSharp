using System;
using System.Diagnostics;

namespace TestObjectCast
{
    interface IInterface
    {
    }

    class Base
    {
    }

    class Base2 : Base
    {
    }

    class Derived : Base2
    {
    }

    class Derived2 : Derived, IInterface
    {
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            Base b = new Derived();
            const int n = 1000000;
            // var d2 = (Derived2)b;
            // Console.WriteLine(d2);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Derived2 d2 = null;
            for (int i = 0; i < n; i++)
            {
                try
                {
                    d2 = (Derived2)b;
                }
                catch (InvalidCastException e)
                {
                }
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            sw.Restart();
            for (int i = 0; i < n; i++)
            {
                d2 = b as Derived2;
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            // Console.WriteLine(d2);
            // Base2 b2 = new Base2();
            // Derived d = new Derived();
            // Derived2 d2 = new Derived2();


            // d2 = d as Derived2;
            // Console.WriteLine(d2);
            // d2 = b2 as Derived2;
            // Console.WriteLine(d2);
            // d2 = b as Derived2;
            // Console.WriteLine(d2);


            // Console.WriteLine(d2);
            // b2 = (Base2)b;
            // Console.WriteLine(b2);
            // d = (Derived)b;
            // Console.WriteLine(d);
        }
    }
}