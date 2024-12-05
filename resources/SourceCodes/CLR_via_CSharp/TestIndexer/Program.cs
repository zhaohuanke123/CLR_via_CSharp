using System.Reflection.Emit;

namespace TestIndexer
{
    class MClass
    {
        public int this[int a]
        {
            get { return a; }
            set { }
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
        }
    }
}