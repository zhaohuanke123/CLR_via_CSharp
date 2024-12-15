using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace TestIsSerializable
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // TestAssemblySer.Go();
            TestChildSer.Go();
        }
    }

    class TestAssemblySer
    {
        public static void Go()
        {
            // 获取当前程序集中导出的所有类型
            Assembly assembly = typeof(int).Assembly;
            Type[] types = assembly.GetExportedTypes();

            Console.WriteLine("Checking serializable types in assembly: " + assembly.FullName);

            foreach (var type in types)
            {
                // 检查是否具有 [Serializable] 特性或实现了 ISerializable 接口
                bool isSerializable = type.IsSerializable || typeof(ISerializable).IsAssignableFrom(type);

                if (!isSerializable)
                {
                    continue;
                }

                Console.WriteLine($"Type: {type.FullName}");
                Console.WriteLine($"  Is Serializable: {isSerializable}");
                Console.WriteLine($"  Is Value Type: {type.IsValueType}");
                Console.WriteLine($"  Is Enum: {type.IsEnum}");
                Console.WriteLine($"  Base Type: {type.BaseType}");
                Console.WriteLine();
            }
        }
    }

    [Serializable]
    internal class Child : Parent, ISerializable
    {
        public int ChildField { get; set; }

        public Child()
        {
        }

        public Child(SerializationInfo info, StreamingContext context)
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }

    internal class Parent
    {
        public int ParentField { get; set; }
    }

    internal static class TestChildSer
    {
        public static void Go()
        {
            Child child = new Child { ChildField = 42, ParentField = 100 };

            try
            {
                // 使用 BinaryFormatter 序列化
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                using (var stream = new System.IO.MemoryStream())
                {
                    formatter.Serialize(stream, child);
                    Console.WriteLine("Serialization succeeded!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Serialization failed: {ex.Message}");
            }
        }
    }
}