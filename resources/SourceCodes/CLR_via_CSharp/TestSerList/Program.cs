using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestSerList
{
    [Serializable]
    internal class Test : ISerializable
    {
        private Test2 test2;
        Test2[] list = new Test2[3];

        public Test()
        {
            test2 = new Test2();
            list[0] = new Test2();
            list[1] = new Test2();
            list[2] = new Test2();
        }

        private Test(SerializationInfo info, StreamingContext context)
        {
            test2 = (Test2)info.GetValue("test2", typeof(Test2));
            list = (Test2[])info.GetValue("list", typeof(Test2[]));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("test2", test2);
            info.AddValue("list", list);
        }

        public override string ToString()
        {
            string s = "test2: " + test2 + "\n";
            s += "list: ";
            foreach (Test2 t in list)
            {
                s += t + " ";
            }

            return s;
        }
    }

    [Serializable]
    internal class Test2
    {
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            Test t = new Test();
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream("test.dat", FileMode.Create))
            {
                bf.Serialize(fs, t);
                fs.Position = 0;
                Test t2 = (Test)bf.Deserialize(fs);
                Console.Write(t2);
            }
        }
    }
}