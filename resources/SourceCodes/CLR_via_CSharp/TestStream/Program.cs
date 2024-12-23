using System.Runtime.Serialization.Formatters.Binary;
using MyNamespace;

SerType type = SerType.Ser;

switch (type)
{
    case SerType.Ser:
        Serialize();
        break;
    case SerType.Deser:
        Deserialize();
        break;
    default:
        break;
}

void Serialize()
{
    using (FileStream fs = new FileStream("test.dat", FileMode.Create))
    {
        BinaryFormatter bf = new BinaryFormatter();
        Test t = new Test();
        bf.Serialize(fs, t);
    }
}

void Deserialize()
{
    using (FileStream fs = new FileStream("test.dat", FileMode.Open))
    {
        BinaryFormatter bf = new BinaryFormatter();
        Test t = (Test)bf.Deserialize(fs);
        Console.WriteLine(t.Heheaaaa);
    }
}

Console.WriteLine("Done");

namespace MyNamespace
{
    [Serializable]
    internal class Test
    {
        public int Heheaaaa = 160;
        private int Hehebbbb = 160;

        protected object HeHeccc = 160;
    }
}

enum SerType
{
    Ser = 0,
    Deser = 1
}