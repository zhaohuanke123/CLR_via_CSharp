using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestTaskCancel
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var task = Task.Run(() =>
            {
                Thread.Sleep(1000);
                cancellationToken.ThrowIfCancellationRequested();
            }, cancellationToken);
            task.ContinueWith((t) => { Console.Out.WriteLine($"Cancel : {Thread.CurrentThread.ManagedThreadId}"); },
                TaskContinuationOptions.OnlyOnCanceled);

            task.Wait();
        }
    }
}