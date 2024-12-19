using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestSurrogateSelector
{
    // 版本 1 的 GameObject 类型
    public class GameObjectV1
    {
        public string Name;
        public int Health;
    }

// 版本 2 的 GameObject 类型（例如，增加了新的属性）
    public class GameObjectV2
    {
        public string Name;
        public int Health;
        public int Armor; // 新增属性
    }

// 模拟的网络连接类型
    public class NetworkConnection
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }
    }

// 序列化代理，用于 GameObjectV1
    public class GameObjectV1SerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            GameObjectV1 gameObject = (GameObjectV1)obj;
            info.AddValue("Name", gameObject.Name);
            info.AddValue("Health", gameObject.Health);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context,
            ISurrogateSelector selector)
        {
            GameObjectV1 gameObject = (GameObjectV1)obj;
            gameObject.Name = info.GetString("Name");
            gameObject.Health = info.GetInt32("Health");
            return gameObject;
        }
    }

// 序列化代理，用于 GameObjectV2
    public class GameObjectV2SerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            GameObjectV2 gameObject = (GameObjectV2)obj;
            info.AddValue("Name", gameObject.Name);
            info.AddValue("Health", gameObject.Health);
            info.AddValue("Armor", gameObject.Armor); // 序列化新属性
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context,
            ISurrogateSelector selector)
        {
            GameObjectV2 gameObject = (GameObjectV2)obj;
            gameObject.Name = info.GetString("Name");
            gameObject.Health = info.GetInt32("Health");
            gameObject.Armor = info.GetInt32("Armor"); // 反序列化新属性
            return gameObject;
        }
    }

// 序列化代理，用于 NetworkConnection 类型（例如，传输连接信息）
    public class NetworkConnectionSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            NetworkConnection connection = (NetworkConnection)obj;
            info.AddValue("IPAddress", connection.IPAddress);
            info.AddValue("Port", connection.Port);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context,
            ISurrogateSelector selector)
        {
            NetworkConnection connection = (NetworkConnection)obj;
            connection.IPAddress = info.GetString("IPAddress");
            connection.Port = info.GetInt32("Port");
            return connection;
        }
    }

    public class GameSurrogateSelector : ISurrogateSelector
    {
        private ISurrogateSelector nextSelector;

        public void ChainSelector(ISurrogateSelector selector)
        {
            this.nextSelector = selector;
        }

        public ISurrogateSelector GetNextSelector()
        {
            return this.nextSelector;
        }

        public ISerializationSurrogate GetSurrogate(Type type, StreamingContext context,
            out ISurrogateSelector selector)
        {
            if (type == typeof(GameObjectV1))
            {
                selector = this;
                return new GameObjectV1SerializationSurrogate();
            }
            else if (type == typeof(GameObjectV2))
            {
                selector = this;
                return new GameObjectV2SerializationSurrogate();
            }
            else if (type == typeof(NetworkConnection))
            {
                selector = this;
                return new NetworkConnectionSerializationSurrogate();
            }

            selector = nextSelector;
            return selector?.GetSurrogate(type, context, out selector);
        }
    }

    class Program
    {
        static void Main()
        {
            // 创建 GameObjectV1 和 GameObjectV2 对象
            GameObjectV1 gameObjectV1 = new GameObjectV1 { Name = "Player1", Health = 100 };
            GameObjectV2 gameObjectV2 = new GameObjectV2 { Name = "Player2", Health = 120, Armor = 50 };

            // 创建 NetworkConnection 对象
            NetworkConnection connection = new NetworkConnection { IPAddress = "192.168.1.1", Port = 8080 };

            // 创建代理选择器链
            GameSurrogateSelector surrogateSelector = new GameSurrogateSelector();

            // 创建序列化流
            MemoryStream memoryStream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();

            // 设置代理选择器
            formatter.SurrogateSelector = surrogateSelector;

            // 序列化对象
            formatter.Serialize(memoryStream, gameObjectV1);
            formatter.Serialize(memoryStream, gameObjectV2);
            formatter.Serialize(memoryStream, connection);

            // 重置流位置
            memoryStream.Seek(0, SeekOrigin.Begin);

            // 反序列化对象
            GameObjectV1 deserializedV1 = (GameObjectV1)formatter.Deserialize(memoryStream);
            GameObjectV2 deserializedV2 = (GameObjectV2)formatter.Deserialize(memoryStream);
            NetworkConnection deserializedConnection = (NetworkConnection)formatter.Deserialize(memoryStream);

            // 输出反序列化后的数据
            Console.WriteLine($"GameObjectV1: {deserializedV1.Name}, Health: {deserializedV1.Health}");
            Console.WriteLine(
                $"GameObjectV2: {deserializedV2.Name}, Health: {deserializedV2.Health}, Armor: {deserializedV2.Armor}");
            Console.WriteLine(
                $"NetworkConnection: IP={deserializedConnection.IPAddress}, Port={deserializedConnection.Port}");
        }
    }
}