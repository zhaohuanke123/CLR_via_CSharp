namespace TestDefartParam
{
    internal class Program
    {
        public struct MyStruct
        {
        }

        public static void Main(string[] args)
        {
            Test();
        }

        public static void Test(MyStruct myStruct = default)
        {
        }
    }
}