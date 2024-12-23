using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using TestStream4;

namespace TestStream4
{
    enum SerType
    {
        Ser = 0,
        Deser = 1
    }

    [Serializable]
    internal class Test
    {
        public int Heheaaaa = 160;
        private int Hehebbbb = 160;

        protected object HeHeccc = 160;
    }

    [Serializable]
    internal class TestSetType : ISerializable
    {
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.SetType(typeof(TestSetTypeHelper));
        }
    }

    [Serializable]
    internal class TestSetTypeHelper : IObjectReference
    {
        public object GetRealObject(StreamingContext context)
        {
            return "Hehehehehe";
        }
    }

    internal class TestSurr : ISerializationSurrogate
    {
        public void GetObjectData(Object obj, SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Ms", ((Ms)obj).Value);
        }

        public Object SetObjectData(Object obj, SerializationInfo info, StreamingContext context,
            ISurrogateSelector selector)
        {
            // var type = obj.GetType();
            // var fieldInfo = type.GetField("m_value");
            // fieldInfo.SetValue(obj, info.GetInt32("Int32"));

            return new Ms(info.GetInt32("Ms"));
        }
    }
}

[Serializable]
internal struct Ms
{
    public int Value;

    public Ms(int value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

internal class Program
{
    public static void Main(string[] args)
    {
        Serialize();
        Deserialize();
        Console.WriteLine("Done");
    }

    static void Serialize()
    {
        using (var fs = new FileStream("test.dat", FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            Ms number = new Ms(5);

            var selector = new SurrogateSelector();
            selector.AddSurrogate(typeof(Ms), formatter.Context, new TestSurr());
            formatter.SurrogateSelector = selector;

            formatter.Serialize(fs, number);
        }
    }

    static void Deserialize()
    {
        using (var fs = new FileStream("test.dat", FileMode.Open))
        {
            var bf = new BinaryFormatter();

            var selector = new SurrogateSelector();
            selector.AddSurrogate(typeof(Ms), bf.Context, new TestSurr());
            bf.SurrogateSelector = selector;

            var t = bf.Deserialize(fs);
            Console.WriteLine(t);
        }
    }
}