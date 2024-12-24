using System;
using System.Collections.Generic;
using System.IO;
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
        private const int t = 1;
        public List<object> List = new List<object>();
    }

    //[Serializable]
    class MyClass2
    {
        private MyClass2 _myClass2;
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var myClass = new MyClass();
            myClass.List.Add(new MyClass2());
            myClass.List.Add(new MyClass2());
            myClass.List.Add(new MyClass2());

            var serializableMembers = FormatterServices.GetSerializableMembers(typeof(MyClass));
            foreach (var member in serializableMembers)
            {
                Console.WriteLine(member);
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, new MyClass());
            memoryStream.Position = 0;
            var deserialize = binaryFormatter.Deserialize(memoryStream);
            Console.WriteLine(deserialize);

            // FormatterServices.GetTypeFromAssembly()
        }
    }
}