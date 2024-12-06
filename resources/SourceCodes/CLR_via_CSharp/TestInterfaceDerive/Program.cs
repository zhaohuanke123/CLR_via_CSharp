using System;

namespace TestInterfaceDerive
{
    interface IA
    {
        void Method();
    }

    interface IB : IA
    {
        new void Method();
    }

    class MyClass : IB
    {
        public void Method()
        {
            Console.WriteLine("MyClass.Method()");
        }
        // void IA.Method()
        // {
        //     
        // }
        //
        // void IB.Method()
        // {
        // }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            MyClass mc = new MyClass();
            IA ia = mc;
            IB ib = mc;
            ia.Method();
            ib.Method();
        }
    }
}