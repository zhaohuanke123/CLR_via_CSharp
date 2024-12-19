using System.Diagnostics;

namespace TestOnDeserialization
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

    class TestCtor
    {
        public TestCtor()
        {
            // Console.WriteLine($"TestCtor {name}");
            // 获取当前调用堆栈的信息
            var stackTrace = new StackTrace();
            var mb = stackTrace.GetFrame(1).GetMethod();
            var type = mb.DeclaringType;
            var typeName = type.Name;

            // Console.WriteLine("当前类: " + typeName + "   当前线程ID : " + Thread.CurrentThread.ManagedThreadId);
            var s = typeName + " " + mb.Name;
            Console.WriteLine(s);
        }
    }

    [Serializable]
    public class Customer : ISerializable
    {
        public string Name { get; set; }
        public Order LastOrder { get; set; }

        public Customer()
        {
        }

        // 特殊构造器：从 SerializationInfo 提取字段
        public Customer(SerializationInfo info, StreamingContext context)
        {
            _ = new TestCtor();
            Name = info.GetString("Name");
            LastOrder = (Order)info.GetValue("LastOrder", typeof(Order));
        }

        // 反序列化后的回调
        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            _ = new TestCtor();
        } // 反序列化后的回调

        // 正常序列化方法
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("LastOrder", LastOrder);
            _ = new TestCtor();
        }
    }

    [Serializable]
    public class Order : ISerializable
    {
        public Orderder Orderder { get; set; }
        public object a;

        public Order()
        {
        }

        public Order(SerializationInfo info, StreamingContext context)
        {
            _ = new TestCtor();
            Orderder = (Orderder)info.GetValue("Orderder", typeof(Orderder));
        }

        // 反序列化后的回调
        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            a = new object();
            _ = new TestCtor();
        } // 反序列化后的回调

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Orderder", Orderder);
            _ = new TestCtor();
        }
    }

    [Serializable]
    public class Orderder : ISerializable
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        public Orderder()
        {
        }

        public Orderder(SerializationInfo info, StreamingContext context)
        {
            _ = new TestCtor();
            OrderId = info.GetInt32("OrderId");
            OrderDate = info.GetDateTime("OrderDate");
        }

        // 反序列化后的回调
        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            _ = new TestCtor();
        } // 反序列化后的回调

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("OrderId", OrderId);
            info.AddValue("OrderDate", OrderDate);
            _ = new TestCtor();
        }
    }

    class Program
    {
        static void Main()
        {
            var ordered = new Orderder
            {
                OrderId = 1,
                OrderDate = DateTime.Now
            };
            // 创建 Order 和 Customer 对象
            var order = new Order
            {
                Orderder = ordered
            };

            var customer = new Customer
            {
                Name = "John Doe",
                LastOrder = order
            };

            // 序列化对象到文件
            string filePath = "customerOrder.dat";
            SerializeObject(customer, filePath);

            try
            {
                // 反序列化对象，触发错误
                Customer deserializedCustomer = DeserializeObject(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during deserialization: " + ex.Message);
            }
        }

        // 序列化对象
        static void SerializeObject(object obj, string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                formatter.Serialize(stream, obj);
            }
        }

        // 反序列化对象
        static Customer DeserializeObject(string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return (Customer)formatter.Deserialize(stream);
            }
        }
    }
}