namespace TestEquals
{
    class EquClass
    {
        private int a;
        private int b;

        public EquClass(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            EquClass a = new EquClass(1, 2);
            EquClass b = new EquClass(1, 2);
            if (a == b)
            {
                System.Console.WriteLine("a == b");
            }

            if (a.Equals(b))
            {
                System.Console.WriteLine("a.Equals(b)");
            }
        }
    }
}