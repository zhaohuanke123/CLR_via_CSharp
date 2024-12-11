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
        private static event Action callBack;

        static void Main(string[] args)
        {
            callBack();
        }
    }
}