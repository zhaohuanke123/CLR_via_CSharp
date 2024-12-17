using System;

namespace TestDisposeAndFinal
{
    class Test : IDisposable
    {
        ~Test()
        {
            Console.WriteLine("Finalize");
        }

        public void Dispose()
        {
            Console.WriteLine("Dispose");
            GC.SuppressFinalize(this);
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            Test t = new Test();
            t.Dispose();
            t = null;

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            Console.ReadLine();
        }
    }
}