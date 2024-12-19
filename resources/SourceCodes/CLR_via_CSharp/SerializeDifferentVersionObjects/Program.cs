using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerializeDifferentVersionObjects
{
// 版本 1 的类
    public class UserV1
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

// 版本 2 的类
    public class UserV2
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }

    public class UserV1Surrogate : ISerializationSurrogate
    {
        // 将 UserV1 转换为 UserV2
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            UserV1 userV1 = (UserV1)obj;

            // 将 UserV1 的数据放到 UserV2 中
            info.AddValue("FullName", userV1.Name);
            info.AddValue("Age", userV1.Age);
            info.AddValue("Email", "unknown@example.com"); // 新版本中的新字段
        }

        // 将序列化的数据反序列化为 UserV2 对象
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context,
            ISurrogateSelector selector)
        {
            UserV2 userV2 = new UserV2();

            userV2.FullName = info.GetString("FullName");
            userV2.Age = info.GetInt32("Age");
            userV2.Email = info.GetString("Email");

            return userV2;
        }
    }

    public class SurrogateSelector1 : ISurrogateSelector
    {
        private ISurrogateSelector nextSelector;

        public void ChainSelector(ISurrogateSelector selector)
        {
            nextSelector = selector;
        }

        public ISurrogateSelector GetNextSelector()
        {
            return nextSelector;
        }

        public ISerializationSurrogate GetSurrogate(Type type, StreamingContext context,
            out ISurrogateSelector selector)
        {
            if (type == typeof(UserV1))
            {
                selector = this;
                return new UserV1Surrogate();
            }

            selector = nextSelector;
            return nextSelector?.GetSurrogate(type, context, out selector);
        }
    }

    public class SurrogateSelector2 : ISurrogateSelector
    {
        private ISurrogateSelector nextSelector;

        public void ChainSelector(ISurrogateSelector selector)
        {
            nextSelector = selector;
        }

        public ISurrogateSelector GetNextSelector()
        {
            return nextSelector;
        }

        public ISerializationSurrogate GetSurrogate(Type type, StreamingContext context,
            out ISurrogateSelector selector)
        {
            if (type == typeof(UserV1)) // 这里可以添加其它类型的支持
            {
                selector = this;
                return new UserV1Surrogate(); // 使用 UserV1Surrogate 来进行版本转换
            }

            selector = nextSelector;
            return nextSelector?.GetSurrogate(type, context, out selector);
        }
    }

    public class Program
    {
        public static void Main()
        {
            // 创建版本 1 的对象
            UserV1 userV1 = new UserV1
            {
                Name = "John Doe",
                Age = 30
            };

            // 创建代理选择器
            SurrogateSelector1 selector1 = new SurrogateSelector1();
            SurrogateSelector2 selector2 = new SurrogateSelector2();

            // 将代理选择器链在一起
            selector1.ChainSelector(selector2);

            // 创建一个流来序列化
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // 设置格式化器
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.SurrogateSelector = selector1;

                // 序列化 UserV1 对象
                formatter.Serialize(memoryStream, userV1);

                // 反序列化，得到 UserV2 对象
                memoryStream.Seek(0, SeekOrigin.Begin); // 重置流的位置
                UserV2 deserializedUserV2 = (UserV2)formatter.Deserialize(memoryStream);

                // 输出结果
                Console.WriteLine($"FullName: {deserializedUserV2.FullName}");
                Console.WriteLine($"Age: {deserializedUserV2.Age}");
                Console.WriteLine($"Email: {deserializedUserV2.Email}");
            }
        }
    }
}