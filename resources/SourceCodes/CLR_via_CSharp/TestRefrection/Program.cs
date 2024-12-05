using System;
using System.Diagnostics;
using System.Reflection;

namespace TestRefrection
{
    internal class Program
    {
        public Program()
        {
            Console.WriteLine("Hello World!");
        }

        public static void Main(string[] args)
        {
            Type t = typeof(Program);
            t.GetCustomAttributesData();
            t.IsDefined(t);
            t.GetCustomAttributes();
            t.GetCustomAttribute(t.GetTypeInfo());
            CustomAttributeData.GetCustomAttributes(t.GetTypeInfo());
        }
    }
}