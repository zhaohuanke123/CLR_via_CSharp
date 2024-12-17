using System;
using System.Reflection;

namespace C23Reflection
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // TestTypeInfo.Go();
            TestLoadReflectionOnly.Go();
        }
    }

    static class TestLoadReflectionOnly
    {
        public static void Go()
        {
            string path = @"Ch01-1-SomeLibrary.dll";
            var assembly = Assembly.ReflectionOnlyLoad(path);

            foreach (var type in assembly.ExportedTypes)
            {
                Console.WriteLine(type);
            }
        }
    }

    static class TestTypeInfo
    {
        public static void Go()
        {
            Type t = typeof(TestTypeInfo);
            var typeInfo = t.GetTypeInfo();
            Console.WriteLine(typeInfo is MemberInfo);
        }
    }

    static class TestIsClassOrIsStruct
    {
        public static void Go()
        {
            Type t = typeof(TestIsClassOrIsStruct);
            var typeInfo = t.GetTypeInfo();
            Console.WriteLine(typeInfo.IsClass);
            Console.WriteLine(typeInfo.IsValueType);
        }
    }
}