using System;
using System.Runtime;

namespace TestServerOrClient
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(GCSettings.IsServerGC);
        }
    }
}