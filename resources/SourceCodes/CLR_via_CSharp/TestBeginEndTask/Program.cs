using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestBeginEndTask
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Action ac = () =>
            {
                Thread.Sleep(1000);
                Console.WriteLine(123);
            };
            var fromAsync = Task.Factory.FromAsync(ac.BeginInvoke(null, null), ac.EndInvoke);
            fromAsync.Wait();
        }
    }
}