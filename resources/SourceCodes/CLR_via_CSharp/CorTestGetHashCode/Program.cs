namespace CorTestGetHashCode
{
    internal class GetHashClass
    {
        private int a;
        private int b;
        public string s;

        public GetHashClass(int a, int b, string s)
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

            GetHashClass a = new GetHashClass(1, 2, "hhhh");
            lock (a)
            {
                var hsah = a.GetHashCode();
                Console.WriteLine(hsah);
            }
        }
    }
}