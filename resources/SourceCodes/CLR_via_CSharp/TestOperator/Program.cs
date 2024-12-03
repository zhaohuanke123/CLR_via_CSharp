namespace TestOperator
{
    internal class Program
    {
        public static bool operator ==(Program a, Program b)
        {
            return true;
        }

        public static bool operator !=(Program a, Program b)
        {
            return !(a == b);
        }

        public static void Main(string[] args)
        {
        }
    }
}