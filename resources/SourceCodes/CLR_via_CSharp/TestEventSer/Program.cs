using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestEventSer
{
    [Serializable]
    class Test
    {
        public event Action<int> callBack;

        public void Call(int i)
        {
            callBack?.Invoke(i);
        }
    }

    [Serializable]
    class Test2
    {
        public void TestInstance(int i)
        {
            Console.WriteLine(i + " " + this);
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            // var test = new Test();
            var test2 = new Test2();
            // test.callBack += test2.TestInstance;
            // test.callBack += test2.TestInstance;
            // test.Call(1);
            Action<int> action = test2.TestInstance;

            var binaryFormatter = new BinaryFormatter();
            var stream = new FileStream("data.txt", FileMode.Create);

            binaryFormatter.Serialize(stream, action);
            stream.Position = 0;
            // stream.Close();
            var deserialize = binaryFormatter.Deserialize(stream) as Action<int>;
            deserialize.Invoke(1);
            // deserialize.Call(1);
        }
    }
}