//  测试什么时候会调用实例构造器

using System.Runtime.Serialization.Formatters.Binary;

namespace TestCtor
{
    [Serializable]
    class MClass
    {
        public MClass()
        {
            Console.WriteLine("mclass ctor");
        }

        public MClass Clone()
        {
            return (MClass)this.MemberwiseClone();
        }
    }

    class Program
    {
        public static void Main()
        {
            MClass mClass = new MClass();
            var clone = mClass.Clone();

            // 测试使用序列化
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, mClass);
                stream.Seek(0, SeekOrigin.Begin);
                var clone2 = (MClass)formatter.Deserialize(stream);
            }
        }
    }
}