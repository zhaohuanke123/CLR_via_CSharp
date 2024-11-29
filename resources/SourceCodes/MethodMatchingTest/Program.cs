using System;

namespace MethodMatchingTest
{
    public static class MethodExtensions
    {
        // 扩展方法
        public static void Print(this TestClass obj, int x)
        {
            Console.WriteLine("Extension Method: Print(this TestClass obj, int x)");
        }

        public static void Print<T>(this TestClass obj, T x)
        {
            Console.WriteLine($"Extension Generic Method: Print<T>(this TestClass obj, T x) with {typeof(T)}");
        }
    }

    public class TestClass
    {
        // 普通实例方法
        //public void Print(int x)
        //{
        //    Console.WriteLine("Instance Method: Print(int x)");
        //}

        public void Print(int x, int y)
        {
            Console.WriteLine("Instance Method: Print(int x, int y)");
        }
        public void Print<T>(T x)
        {
            Console.WriteLine($"Generic Instance Method: Print<T>(T x) with {typeof(T)}");
        }

        // 可变参数方法
        public void Print(params object[] x)
        {
            Console.WriteLine("Params Method: Print(params object[] x)");
        }

        // 重载方法
        public void Print(double x)
        {
            Console.WriteLine("Instance Method: Print(double x)");
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            TestClass obj = new TestClass();

            Console.WriteLine("=== Test 1: Passing an int ===");
            obj.Print(10);

            Console.WriteLine("\n=== Test 2: Passing a double ===");
            obj.Print(10.0);

            Console.WriteLine("\n=== Test 3: Passing an array ===");
            obj.Print(1, 2);

            Console.WriteLine("\n=== Test 4: Using Extension Method ===");
            //((MethodExtensions)obj).Print(10);

            Console.WriteLine("\n=== Test 5: Using Generic Extension Method ===");
            obj.Print("Hello");
        }
    }
}
