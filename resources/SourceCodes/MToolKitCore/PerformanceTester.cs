using System.Diagnostics;

namespace MToolKitCore
{
    public class PerformanceTester : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly string _testName;

        public PerformanceTester(string testName = "Code Block")
        {
            //GC.Collect(2, GCCollectionMode.Forced);
            _testName = testName;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            Console.WriteLine($"[{_testName}] Completed in {_stopwatch.Elapsed.TotalMilliseconds} ms.");
        }
    }
}
