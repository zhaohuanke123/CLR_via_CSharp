namespace TestSerSelector
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

    public class Program
    {
        public static void Main()
        {
            // 创建并配置代理选择器
            SurrogateSelector selector1 = new SurrogateSelector();
            SurrogateSelector selector2 = new SurrogateSelector();

            // 在 selector1 中添加代理，用于序列化 MyType
            selector1.AddSurrogate(typeof(MyType), new StreamingContext(StreamingContextStates.All),
                new MyTypeSurrogate());

            // 在 selector2 中添加代理，用于序列化 MyTypeV2
            selector2.AddSurrogate(typeof(MyTypeV2), new StreamingContext(StreamingContextStates.All),
                new MyTypeV2Surrogate());

            // 链接两个选择器
            selector1.ChainSelector(selector2);

            // 创建一个对象并序列化
            MyType myTypeObj = new MyType { MyValue = 42 };
            byte[] serializedData = SerializeObject(myTypeObj, selector1);

            // 反序列化并输出结果
            MyType deserializedObj = (MyType)DeserializeObject(serializedData, selector1);
            Console.WriteLine($"Deserialized MyTypeV2: MyValue = {deserializedObj.MyValue}");
        }

        // 序列化方法
        public static byte[] SerializeObject(object obj, SurrogateSelector selector)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.SurrogateSelector = selector;
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        // 反序列化方法
        public static object DeserializeObject(byte[] serializedData, SurrogateSelector selector)
        {
            using (MemoryStream stream = new MemoryStream(serializedData))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.SurrogateSelector = selector;
                return formatter.Deserialize(stream);
            }
        }
    }

// 原始类型 MyType
    [Serializable]
    public class MyType
    {
        public int MyValue { get; set; }
    }

// 版本 2 类型 MyTypeV2
    [Serializable]
    public class MyTypeV2
    {
        public int MyValue { get; set; }
    }

// MyType 的序列化代理
    public class MyTypeSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            MyType myType = (MyType)obj;
            info.AddValue("MyValue", myType.MyValue);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context,
            ISurrogateSelector selector)
        {
            MyType myType = (MyType)obj;
            myType.MyValue = info.GetInt32("MyValue");
            return myType;
        }
    }

// MyTypeV2 的序列化代理
    public class MyTypeV2Surrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            MyTypeV2 myTypeV2 = (MyTypeV2)obj;
            info.AddValue("MyValue", myTypeV2.MyValue);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context,
            ISurrogateSelector selector)
        {
            MyTypeV2 myTypeV2 = (MyTypeV2)obj;
            myTypeV2.MyValue = info.GetInt32("MyValue");
            return myTypeV2;
        }
    }
}