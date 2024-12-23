using System;
using System.IO;
using System.Reflection;

namespace TestReflectionTrhow
{
    class Test
    {
        void TestThrow()
        {
            throw new Exception();
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var type = typeof(Test);
            var test = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod("TestThrow", BindingFlags.Instance | BindingFlags.NonPublic);
            methodInfo.Invoke(test, null);
        }
    }
}
