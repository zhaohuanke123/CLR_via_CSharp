using System.IO;

namespace TestFileStream
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            FileStream fileStream = new FileStream("log.txt", FileMode.Create);
        }
    }
}