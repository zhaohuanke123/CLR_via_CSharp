using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CrossAppDomainProxy
{
    public interface IMyObject
    {
        string GetData();
    }

    [Serializable]
    public class MyObject : IMyObject
    {
        public string Data { get; set; }

        public MyObject(string data)
        {
            Data = data;
        }

        public string GetData()
        {
            return Data;
        }
    }

    public class MyObjectProxy : IMyObject
    {
        private IMyObject _realObject;

        public MyObjectProxy(IMyObject realObject)
        {
            _realObject = realObject;
        }

        public string GetData()
        {
            Console.WriteLine("Proxy: Fetching data through proxy...");
            return _realObject.GetData();
        }
    }

    public class MyObjectSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            MyObject myObject = (MyObject)obj;
            info.AddValue("Data", myObject.Data);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context,
            ISurrogateSelector selector)
        {
            MyObject myObject = (MyObject)obj;
            myObject.Data = info.GetString("Data");
            return myObject;
        }
    }

    public class MySurrogateSelector : ISurrogateSelector
    {
        private ISurrogateSelector _nextSelector;

        public void ChainSelector(ISurrogateSelector selector)
        {
            _nextSelector = selector;
        }

        public ISurrogateSelector GetNextSelector()
        {
            return _nextSelector;
        }

        public ISerializationSurrogate GetSurrogate(Type type, StreamingContext context,
            out ISurrogateSelector selector)
        {
            if (type == typeof(MyObject))
            {
                selector = this;
                return new MyObjectSurrogate();
            }

            selector = null;
            return null;
        }
    }

    class Program
    {
        static void Main()
        {
            // 创建一个 MyObject 实例
            MyObject originalObject = new MyObject("Some data");

            // 创建一个代理对象
            MyObjectProxy proxy = new MyObjectProxy(originalObject);

            // 将 MyObject 用序列化代理（Surrogate）进行序列化
            MySurrogateSelector surrogateSelector = new MySurrogateSelector();
            IFormatter formatter = new BinaryFormatter();
            formatter.SurrogateSelector = surrogateSelector;

            using (MemoryStream stream = new MemoryStream())
            {
                // 序列化对象
                formatter.Serialize(stream, originalObject);

                // 模拟反序列化并恢复代理
                stream.Seek(0, SeekOrigin.Begin);
                MyObject deserializedObject = (MyObject)formatter.Deserialize(stream);

                // 打印反序列化后的数据
                Console.WriteLine("Deserialized Object Data: " + deserializedObject.GetData());
            }

            // 使用代理对象调用方法
            Console.WriteLine("Using Proxy:");
            Console.WriteLine(proxy.GetData());
        }
    }
}