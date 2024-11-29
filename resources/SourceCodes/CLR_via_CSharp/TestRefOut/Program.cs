namespace TestRefOut
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            int a = 1;
            Test(ref a);
            Test1(out a);
        }

        public static void Test(ref int a)
        {
        }

        public static void Test1(out int a)
        {
            a = 1;
        }
    }
}