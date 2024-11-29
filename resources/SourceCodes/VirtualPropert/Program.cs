namespace VirtualPropert
{
    public class Base
    {
        public virtual int S { get; set; }

        public virtual void Test()
        {

        }
    }

    public class Derive : Base
    {
        public sealed override int S { get; set; }

        public sealed override void Test()
        {
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
