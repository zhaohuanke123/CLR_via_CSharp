using System.Text;

namespace TestSB
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder(0, 10);
            sb.Append("1234567890");
            sb.Append("1234567890");
        }
    }
}