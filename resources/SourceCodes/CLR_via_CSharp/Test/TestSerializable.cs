using System;
using System.Runtime.Serialization;

[Serializable]
public class SerializableField : ISerializable
{
    public SerializableField()
    {
    }

    public SerializableField(SerializationInfo info, StreamingContext context)
    {
        Console.WriteLine("SerializableField.ctor");
    }


    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        Console.WriteLine("SerializableField.GetObjectData");
    }
}

[Serializable]
public class MyClass : ISerializable
{
    private string name;
    private DateTime creationDate;
    private SerializableField serializableField; // 假设它实现了 ISerializable

    public MyClass(string name, DateTime date, SerializableField field)
    {
        this.name = name;
        this.creationDate = date;
        this.serializableField = field;
    }

    // 用于序列化的构造函数
    protected MyClass(SerializationInfo info, StreamingContext context)
    {
        name = info.GetString("Name");
        creationDate = info.GetDateTime("CreationDate");
        serializableField = (SerializableField)info.GetValue("SerializableField", typeof(SerializableField));
    }

    // 实现 ISerializable 接口的 GetObjectData 方法
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        // 使用 AddValue 方法添加序列化信息
        info.AddValue("Name", name);
        info.AddValue("CreationDate", creationDate);

        // 不要调用 serializableField.GetObjectData
        info.AddValue("SerializableField", serializableField); // 格式化器会自动调用 serializableField 的 GetObjectData
    }
}

public class TestSerializable
{
    public static void Go()
    {
        // 测试序列化
        MyClass myObject = new MyClass("My Object", DateTime.Now, new SerializableField());
        Console.WriteLine("Object created");

        // 创建一个流来保存序列化数据
        using (System.IO.Stream stream = new System.IO.FileStream("data.xml", System.IO.FileMode.Create,
                   System.IO.FileAccess.Write, System.IO.FileShare.None))
        {
            // 创建一个 BinaryFormatter 对象来执行序列化
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(stream, myObject);
        }

        // 测试反序列化
        MyClass newObject;
        using (System.IO.Stream stream = new System.IO.FileStream("data.xml", System.IO.FileMode.Open,
                   System.IO.FileAccess.Read, System.IO.FileShare.None))
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            newObject = (MyClass)formatter.Deserialize(stream);
        }

        Console.WriteLine(newObject);
        Console.WriteLine("Object deserialized");
    }
}