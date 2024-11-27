using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Console = System.Console;

namespace TestNewAndVirtual
{
    class A
    {
        public virtual void Test()
        {
            Console.WriteLine("A ");
        }
    }

    class B : A
    {
        public override void Test()
        {
            Console.WriteLine("B : ");
        }
    }

    class C : B
    {
        public new void Test()
        {
            Console.WriteLine("C : ");
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            A a = new C();
            a.Test();
        }
    }
}
