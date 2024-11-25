namespace TestStack
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var a = 1;
            var b = 2;
            var c = Add(a, b);
            System.Console.WriteLine($"{a} + {b} = {c}");
        }
        
        public static int Add(int a, int b)
        {
            return a + b;
        }
    }
}