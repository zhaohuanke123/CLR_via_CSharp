//https://learn.microsoft.com/zh-cn/dotnet/csharp/nullable-references

namespace TestNullable
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int? a = 10;
            int b = 10;
            Console.WriteLine(a == b);
        }
    }
}
