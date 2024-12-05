using System.Diagnostics;

namespace TestCoreRuntime
{
    struct MyStruct
    {
        public MyStruct(int a)
        {
        }
    }

    class MyClass
    {
        public MyClass()
        {
        }

        public override int GetHashCode()
        {
            return 123;
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            Object o = new MyClass();

            Console.WriteLine(o.ToString());
            // Console.WriteLine(typeof(Object).Assembly.GetName());
            Console.ReadLine();
        }
    }
}