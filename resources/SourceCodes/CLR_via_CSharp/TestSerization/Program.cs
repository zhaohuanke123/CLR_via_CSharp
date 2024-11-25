using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestSerization
{
    internal class Program
    {
        [Serializable]
        class Se1 : ISerializable, IDeserializationCallback
        {
            public InterSe interSe;
            public SerializationInfo info;

            public Se1()
            {
                interSe = new InterSe();
            }

            public Se1(SerializationInfo info, StreamingContext context)
            {
                this.info = info;
                interSe = (InterSe)info.GetValue("interSe", typeof(InterSe));
                Console.WriteLine("Se1 Constructor");
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("interSe", interSe);
                Console.WriteLine("Se1 GetObjectData");
            }

            public void OnDeserialization(object sender)
            {
            }
        }

        [Serializable]
        class InterSe : ISerializable
        {
            public Inter2Se inter2Se;

            public InterSe()
            {
                inter2Se = new Inter2Se();
            }

            public InterSe(SerializationInfo info, StreamingContext context)
            {
                inter2Se = (Inter2Se)info.GetValue("inter2Se", typeof(Inter2Se));
                Console.WriteLine("InterSe Constructor");
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("inter2Se", inter2Se);
                Console.WriteLine("InterSe GetObjectData");
            }
        }

        [Serializable]
        class Inter2Se : ISerializable
        {
            public Inter2Se()
            {
            }

            public Inter2Se(SerializationInfo info, StreamingContext context)
            {
                Console.WriteLine("Inter2Se Constructor");
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                Console.WriteLine("Inter2Se GetObjectData");
            }
        }

        public static void Main(string[] args)
        {
            Se1 se1 = new Se1();
            Console.WriteLine(se1);
            Se1 obj = (Se1)DeepClone(se1);
            Console.WriteLine(obj);
            Console.WriteLine(obj.interSe);
            Console.WriteLine(obj.interSe.inter2Se);
        }

        public static Object DeepClone(Object obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, obj);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return binaryFormatter.Deserialize(memoryStream);
            }
        }
    }
}