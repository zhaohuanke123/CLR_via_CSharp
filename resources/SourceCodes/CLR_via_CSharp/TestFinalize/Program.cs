using System;

namespace TestFinalize
{
    [Serializable]
    internal class Program
    {

        public static void Main(string[] args)
        {
            _ = new Program();
            _ = new Program();
            _ = new Program();
            Console.WriteLine("Hello World!");
            GC.Collect(2, GCCollectionMode.Forced, true);
            GC.Collect(2, GCCollectionMode.Forced, true);
            GC.Collect(2, GCCollectionMode.Forced, true);
            GC.Collect(2, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();
            try
            {
                Console.WriteLine("Done");
                throw new Exception();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // throw;
            }
        }

        ~Program()
        {
            Console.WriteLine("Finalized");
        }
    }
}