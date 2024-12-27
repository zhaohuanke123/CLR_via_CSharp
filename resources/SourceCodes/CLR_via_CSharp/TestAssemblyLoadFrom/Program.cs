using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace TestAssemblyLoadFrom
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // string path =
            //     @"D:\Study\CLR_via_CSharp\resources\SourceCodes\CLR_via_CSharp\bin\Debug\Ch01-1-SomeLibrary.dll";
            // string path1 =
            //     @"D:\Study\CLR_via_CSharp\resources\SourceCodes\CLR_via_CSharp\bin\Release\Ch01-1-SomeLibrary.dll";
            //
            // var assembly1 = Assembly.LoadFrom(path);
            // Console.WriteLine(assembly1);
            // var assembly2 = Assembly.LoadFrom(path1);
            // Console.WriteLine(assembly2);
            // Console.WriteLine(ReferenceEquals(assembly1, assembly2));

            // Type t = typeof(Program);
            // var tModule = t.Module;
            // Console.WriteLine(tModule);
            // var type = Type.GetType("System.Reflection.PropertyInfo, mscorlib");
            // Console.WriteLine(type);
            // FormatterServices.PopulateObjectMembers(null, null, null);
        }
    }
}