using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestSerialSame
{
    [Serializable]
    class ClassA
    {
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            ClassA a = new ClassA();
            ClassA[] arr =
            {
                a, a
            };

            BinaryFormatter bf = new BinaryFormatter();
            var stream = new MemoryStream();
            bf.Serialize(stream, arr);

            stream.Position = 0;
            var arr2 = (ClassA[])bf.Deserialize(stream);
            Console.WriteLine(ReferenceEquals(arr2[0], arr2[1]));
        }
    }
}