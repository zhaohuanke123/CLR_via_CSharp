using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestTaskScheduler
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            var cts = new CancellationTokenSource();
            var tf = new TaskFactory<int>(cts.Token, TaskCreationOptions.AttachedToParent,
                TaskContinuationOptions.ExecuteSynchronously, scheduler);

            tf.StartNew(() =>
            {
                Console.Out.WriteLine($"Start !");
                return 0;
            }, cts.Token);
        }
    }
}