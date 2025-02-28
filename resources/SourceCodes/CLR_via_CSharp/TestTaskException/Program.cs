using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace TestTaskException
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var task = Task.Run(() =>
                {
                    Task t1 = new Task(() =>
                        {
                            Thread.Sleep(2000);
                            throw new ArgumentException("Task exception");
                        },
                        TaskCreationOptions.AttachedToParent);
                    Task t2 = new Task(() => { throw new DataException("Data exception"); },
                        TaskCreationOptions.AttachedToParent);
                    Task t3 = new Task(() => { throw new InvalidOperationException("Invalid operation exception"); },
                        TaskCreationOptions.AttachedToParent);
                    t1.Start();
                    t2.Start();
                    t3.Start();

                    // Thread.Sleep(1000);
                    // throw new NullReferenceException();
                    Task.WaitAll(t1, t2, t3);
                });
                task.Wait();
            }
            catch (Exception e)
            {
                if (e is AggregateException aggregateException)
                {
                    Console.Out.WriteLine(aggregateException.InnerExceptions.Count);
                    foreach (Exception innerException in aggregateException.InnerExceptions)
                    {
                        Console.Out.WriteLine("Exception: " + innerException);
                    }
                }

                Console.Out.WriteLine(e.InnerException);
            }
        }
    }
}