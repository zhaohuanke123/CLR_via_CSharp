namespace CoreTestEquals
{
    internal struct EquClass
    {
        private int a;
        private int b;
        public string s;

        public EquClass(int a, int b,string s)
        {
            this.a = a;
            this.b = b;
            this.s = s;
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            EquClass a = new EquClass(1, 2, "hhhh");
            EquClass b = new EquClass(1, 2, new string('h', 4));
            Object o1 = a;
            Object o2 = b;

            if (a.Equals(b))
            {
                Console.WriteLine("a.Equals(b)");
            }
            else
            {
                Console.WriteLine("a does not equal b");
            }
            
            if (o1.Equals(o2))
            {
                Console.WriteLine("o1.Equals(o2)");
            }
            else
            {
                Console.WriteLine("o1 does not equal o2");
            }
        }
    }
}