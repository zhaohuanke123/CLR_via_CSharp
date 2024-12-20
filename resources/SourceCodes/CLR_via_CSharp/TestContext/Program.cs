using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestContext
{
    [Serializable]
    class Class : ISerializable
    {
        public Class()
        {
        }

        public Class(SerializationInfo info, StreamingContext context)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            Test1();
            Test2();
        }

        private static void Test2()
        {
            using (FileStream fs = new FileStream("data.txt", FileMode.Open))
            {
                BinaryFormatter bf =
                    new BinaryFormatter();
                Class c = (Class)bf.Deserialize(fs);
            }
        }

        private static void Test1()
        {
            // 序列哈到文件中
            Class c = new Class();
            using (FileStream fs = new FileStream("data.txt", FileMode.Create))
            {
                BinaryFormatter bf =
                    new BinaryFormatter();
                bf.Context = new StreamingContext(StreamingContextStates.File);
                bf.Serialize(fs, c);
            }
        }
    }
}