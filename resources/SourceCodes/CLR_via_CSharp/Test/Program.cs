using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Text;

namespace Test1
{
    // internal class Program
    // {
    //     public static void Main()
    //     {
    //         var assembly = Assembly.ReflectionOnlyLoadFrom(
    //             @"D:\Study\CLR_via_CSharp\resources\SourceCodes\CLR_via_CSharp\bin\Debug\Ch01-1-SomeLibrary.dll");
    //         var type = assembly.GetType("SomeLibrary.SomeLibraryType");
    //         var o = Activator.CreateInstance(type);
    //         Console.WriteLine(o);
    //     }
    // }
    class Test
    {
        public FileStream fs = new FileStream("test.txt", FileMode.Create);

        ~Test()
        {
            fs.Close();
            Console.WriteLine("Finalized");
        }
    }

    public class AssemblyLoadTest
    {
        public static void Main()
        {
            var test = new Test();
            test.fs.Write(Encoding.ASCII.GetBytes("Hello, World!"), 0, 13);

            Console.ReadLine();
        }
    }
}