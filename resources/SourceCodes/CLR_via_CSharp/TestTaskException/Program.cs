using System;
using System.Data;
using System.Threading.Tasks;

namespace TestTaskException
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Task t1 = Task.Run(() => { throw new ArgumentException("Task exception"); });
                Task t2 = Task.Run(() => { throw new DataException("Data exception"); });
                Task t3 = Task.Run(() => { throw new InvalidOperationException("Invalid operation exception"); });

                Task.WaitAll(t1, t2, t3);
            }
            catch (AggregateException e)
            {
                Console.Out.WriteLine(e.InnerExceptions.Count);
                foreach (Exception innerException in e.InnerExceptions)
                {
                    Console.Out.WriteLine("Exception: " + innerException);
                }

                Console.Out.WriteLine(e.InnerException);
            }
        }
    }
}