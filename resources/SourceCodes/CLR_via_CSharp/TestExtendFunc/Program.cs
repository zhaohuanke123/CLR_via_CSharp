namespace TestExtendFunc
{
    internal static class Class
    {
        public static void Test(this Program p)
        {
        }
    }

    internal static class Class1
    {
        public static void Test(this Program p)
        {
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            Program p = new Program();
            Class.Test(p);
            Class1.Test(p);
        }
    }
}