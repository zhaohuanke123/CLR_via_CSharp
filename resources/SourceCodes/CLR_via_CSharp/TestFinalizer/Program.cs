using System;

namespace TestFinalizer
{
    class Base
    {
        ~Base()
        {
            Console.WriteLine("Base Finalizer");
        }
    }

    class Derived : Base
    {
        ~Derived()
        {
            throw new Exception();
            Console.WriteLine("Derived Finalizer");
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            Derived derived = new Derived();
            derived = null;
            try
            {
                GC.Collect();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            Console.ReadLine();
        }
    }
}