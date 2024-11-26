using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MToolKit
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
    public class PerformanceTester : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly string _testName;

        public PerformanceTester(string testName = "Code Block")
        {
            GC.Collect(2, GCCollectionMode.Forced);
            _testName = testName;
            _stopwatch = Stopwatch.StartNew();
            Console.WriteLine($"[{_testName}] Started...");
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            Console.WriteLine($"[{_testName}] Completed in {_stopwatch.Elapsed.TotalMilliseconds} ms.");
        }
    }
}
