using System;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestSerProgress
{
    [Serializable]
    class MyClass
    {
        private int a;
        private string b;
        private MyClass2 MyClass2;
    }

    class MyClass2
    {
        private MyClass2 _myClass2;
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var members = FormatterServices.GetSerializableMembers(typeof(MyClass));
            foreach (var info in members)
            {
                Console.WriteLine(info.GetType());
            }
            // FormatterServices.GetTypeFromAssembly()
        }
    }
}