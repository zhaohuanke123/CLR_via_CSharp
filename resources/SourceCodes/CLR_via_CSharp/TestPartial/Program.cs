using System.Diagnostics;

namespace TestPartial
{
    interface IA
    {
        void A();
    }

    interface IB
    {
        void B();
    }

    abstract partial class MC : Program, IA
    {
        public void B()
        {
        }
    }

    partial class MC : IB
    {
        public void A()
        {
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
        }
    }
}