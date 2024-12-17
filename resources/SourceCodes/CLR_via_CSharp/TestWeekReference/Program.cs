using System;

namespace TestWeekReference
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            WeakReference<Program> weakReference = new WeakReference<Program>(new Program());
            GC.Collect(2, GCCollectionMode.Forced);

            Console.WriteLine(weakReference.TryGetTarget(out _) ? "Target is alive" : "Target is dead");
        }
    }
}