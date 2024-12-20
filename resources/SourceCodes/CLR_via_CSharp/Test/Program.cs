using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;

namespace Test
{
    internal class Program
    {
        public static void Main()
        {
            var assembly = Assembly.ReflectionOnlyLoadFrom(
                @"D:\Study\CLR_via_CSharp\resources\SourceCodes\CLR_via_CSharp\bin\Debug\Ch01-1-SomeLibrary.dll");
            var type = assembly.GetType("SomeLibrary.SomeLibraryType");
            var o = Activator.CreateInstance(type);
            Console.WriteLine(o);
        }
    }
}