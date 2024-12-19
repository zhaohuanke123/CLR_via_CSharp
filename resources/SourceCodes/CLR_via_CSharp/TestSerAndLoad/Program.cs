namespace TestSerAndLoad
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

    class Program
    {
        static void Main()
        {
            // 测试前，确保有一个程序集文件可以用来加载
            string assemblyPath =
                @"D:\Study\CLR_via_CSharp\resources\SourceCodes\CLR_via_CSharp\bin\Debug\Ch01-1-SomeLibrary.dll"; // 替换为实际程序集文件路径
            string dataFilePath = @"serializedData.dat"; // 替换为实际序列化数据路径

            // 注册事件处理程序
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;

            var assembly = Assembly.LoadFrom(assemblyPath);
            var type = assembly.GetType("SomeLibrary.SomeLibraryType");

            // 创建并序列化对象
            object obj = Activator.CreateInstance(type);
            SerializeObject(obj, dataFilePath);

            // 反序列化对象
            object deserializedObj = DeserializeObject(dataFilePath);

            // 输出结果验证
            // Console.WriteLine($"Deserialized Object: Name={deserializedObj}, Value={deserializedObj}");
            Console.WriteLine(ReferenceEquals(obj, deserializedObj));
        }

        // 序列化对象
        static void SerializeObject(object obj, string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                formatter.Serialize(stream, obj);
            }

            Console.WriteLine("Object serialized successfully.");
        }

        // 反序列化对象
        static object DeserializeObject(string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return formatter.Deserialize(stream);
            }
        }

        // 处理程序集解析失败
        static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            Console.WriteLine($"Resolving assembly: {args.Name}");

            // 提取程序集文件名并构建路径
            string assemblyName = new AssemblyName(args.Name).Name + ".dll";
            string assemblyPath =
                @"D:\Study\CLR_via_CSharp\resources\SourceCodes\CLR_via_CSharp\bin\Debug\Ch01-1-SomeLibrary.dll"; // 替换为实际程序集文件路径

            // 检查是否存在该文件
            if (File.Exists(assemblyPath))
            {
                // 加载并返回程序集
                return Assembly.LoadFrom(assemblyPath);
            }
            else
            {
                Console.WriteLine($"Assembly not found: {assemblyName}");
                return null; // 或者抛出异常
            }
        }
    }
}