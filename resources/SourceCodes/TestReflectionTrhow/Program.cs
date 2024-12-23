using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
