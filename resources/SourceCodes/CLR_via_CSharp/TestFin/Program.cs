using System.Text;

namespace TestFin
{
    class Test
    {
        public FileStream fs = new FileStream("test.txt", FileMode.Create);

        ~Test()
        {
            fs.Close();
            Console.WriteLine("Finalized");
        }
    }

    public class AssemblyLoadTest
    {
        public static void Main()
        {
            var test = new Test();
            test.fs.Write(Encoding.ASCII.GetBytes("Hello, World!"), 0, 13);

            GC.WaitForPendingFinalizers();
            Console.ReadLine();
        }
    }
}