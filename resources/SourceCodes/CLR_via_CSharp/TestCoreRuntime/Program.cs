using System.Runtime.InteropServices;

namespace TestCoreRuntime
{
    public class Program
    {
        static void Main(string[] args)
        {
            object o = new object();
            object o1 = new object();
            o = null;
            GCHandle h = GCHandle.Alloc(o, GCHandleType.Weak);
            GC.Collect(0);
            Console.WriteLine(h.Target == null);
            Console.WriteLine(o1);
        }
    }
}