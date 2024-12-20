using System;
using System.Collections.Generic;
using System.Reflection;

namespace TestVersionType
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var type = typeof(Program);
            Console.WriteLine($"{type.FullName}, {type.Assembly}");

            type = typeof(List<Program>);
            Console.WriteLine($"{type.FullName}, {type.Assembly}");

            type = Type.GetType(
                "System.Collections.Generic.List`1[[TestVersionType.Program, TestVersionType, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]");
            Console.WriteLine(type.ToString());
        }
    }
}