namespace TestCheckUnCheck
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            object o = null;
            var t = checked((Program)o);
        }
    }
}