using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestSerialization
{
    class MyClass
    {
        public MyClass()
        {
            Console.WriteLine("MyClass Ctor");
        }
    }

    struct MyStruct
    {
        public int a;
        public int b;
    }

    [Serializable]
    internal class TestClass
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [NonSerialized] public MyClass _nonSerializedField;
        [NonSerialized] public MyStruct _MyStruct;
    }

    class Program
    {
        static void Main()
        {
            // 创建一个对象并设置字段值
            var obj = new TestClass
            {
                Id = 1,
                Name = "Test Object",
                _nonSerializedField = new MyClass(),
                _MyStruct = new MyStruct(),
            };

            // 序列化对象
            IFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                // 序列化对象到流
                formatter.Serialize(stream, obj);

                // 重置流位置
                stream.Position = 0;

                // 反序列化对象
                var deserializedObj = (TestClass)formatter.Deserialize(stream);

                // 输出结果
                Console.WriteLine("Id: " + deserializedObj.Id); // 1
                Console.WriteLine("Name: " + deserializedObj.Name); // Test Object
                Console.WriteLine("NonSerializedField: " + deserializedObj._nonSerializedField); // null
                Console.WriteLine("NonSerializedField: " + deserializedObj._MyStruct); // null
            }
        }
    }
}