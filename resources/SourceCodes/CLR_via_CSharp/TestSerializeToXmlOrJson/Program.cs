namespace TestSerializeToXmlOrJson
{
    using System;
    using System.Xml.Serialization;
    using System.IO;
    using Newtonsoft.Json;

    public class Person
    {
        // 自动属性
        public string Name { get; set; }
        public int Age { get; set; }

        public Person(string name, int age)
        {
            this.Name = name;
            this.Age = age;
            Console.WriteLine("Person Created");
        }

        // 默认构造方法，用于反序列化
        public Person()
        {
            Console.WriteLine("Person Constructor");
        }
    }

    class Program
    {
        static void Main()
        {
            // 创建一个 Person 对象
            var person = new Person( "John Doe", 30);

            // XML 序列化与反序列化
            Console.WriteLine("XML 序列化：");
            string xml = SerializeToXml(person);
            Console.WriteLine(xml);
            Person deserializedFromXml = DeserializeFromXml(xml);
            Console.WriteLine($"从 XML 反序列化: {deserializedFromXml.Name}, {deserializedFromXml.Age}");

            // JSON 序列化与反序列化
            Console.WriteLine("\nJSON 序列化：");
            string json = SerializeToJson(person);
            Console.WriteLine(json);
            Person deserializedFromJson = DeserializeFromJson(json);
            Console.WriteLine($"从 JSON 反序列化: {deserializedFromJson.Name}, {deserializedFromJson.Age}");
        }

        // XML 序列化
        public static string SerializeToXml(Person person)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Person));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, person);
                return writer.ToString();
            }
        }

        // XML 反序列化
        public static Person DeserializeFromXml(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Person));
            using (StringReader reader = new StringReader(xml))
            {
                return (Person)serializer.Deserialize(reader);
            }
        }

        // JSON 序列化
        public static string SerializeToJson(Person person)
        {
            return JsonConvert.SerializeObject(person, Formatting.Indented);
        }

        // JSON 反序列化
        public static Person DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Person>(json);
        }
    }
}