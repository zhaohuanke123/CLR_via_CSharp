using System;
using System.Threading;
using System.Threading.Tasks;

namespace C20TestException
{
    using System;
    using System.Runtime.Serialization;

    abstract class Test
    {
        protected abstract int It { get; set; }
        protected abstract event Action Cb;
    }

// 确保自定义异常类是可序列化的
    [Serializable]
    public class MyCustomException : Exception
    {
        // 可选的自定义属性
        public string AdditionalInformation { get; private set; }

        // 无参数的构造函数
        public MyCustomException()
            : base("A default exception has occurred.")
        {
        }

        // 带有错误消息的构造函数
        public MyCustomException(string message, string additionalInformation)
            : base(message)
        {
            AdditionalInformation = additionalInformation;
        }

        // 带有错误消息和内部异常的构造函数
        public MyCustomException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // 反序列化构造函数
        protected MyCustomException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            AdditionalInformation = info.GetString("AdditionalInformation");
        }

        // 覆盖GetObjectData方法以支持序列化
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("AdditionalInformation", AdditionalInformation);
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            TestAppdomain.Go();
            TestInTaskException.Go();
            TestUnobservedTaskException.Go();
            TestFinally.Go();
        }
    }

    class TestAppdomain
    {
        public static void Go()
        {
            // Create a new AppDomain
            AppDomain newDomain = AppDomain.CreateDomain("NewAppDomain");

            try
            {
                // Execute the method in the new AppDomain
                newDomain.DoCallBack(ThrowExceptionInNewDomain);
            }
            catch (MyCustomException e)
            {
                Console.WriteLine($"Caught exception from another AppDomain: {e.Message}, {e.AdditionalInformation}");
            }
            finally
            {
                Console.WriteLine("Finally");
                AppDomain.Unload(newDomain);
            }
        }

        private static void ThrowExceptionInNewDomain()
        {
            try
            {
                throw new MyCustomException("Exception in new AppDomain", "Additional Info");
            }
            catch (Exception e)
            {
                // Re-throw the exception to be caught in the original AppDomain
                throw new MyCustomException();
            }
        }
    }

    class TestInTaskException
    {
        public static void Go()
        {
            try
            {
                // Start a new task
                var task = Task.Factory.StartNew(() =>
                {
                    throw new MyCustomException("Exception in task", "Additional Info");
                });

                // Wait for the task to complete
                // task.Wait();
                task.ContinueWith(t =>
                {
                    if (t.Exception != null)
                    {
                        foreach (Exception innerException in t.Exception.InnerExceptions)
                        {
                            if (innerException is MyCustomException customException)
                            {
                                Console.WriteLine(
                                    $"Caught exception from task: {customException.Message}, {customException.AdditionalInformation}");
                            }
                        }
                    }
                }, TaskContinuationOptions.OnlyOnFaulted);
            }
            catch (AggregateException e)
            {
                // Handle the exception
                foreach (Exception innerException in e.InnerExceptions)
                {
                    if (innerException is MyCustomException customException)
                    {
                        Console.WriteLine(
                            $"Caught exception from task: {customException.Message}, {customException.AdditionalInformation}");
                    }
                }
            }
            finally
            {
                Console.WriteLine("Finally");
            }

            GC.Collect();
            Console.ReadLine();
        }
    }

    class TestUnobservedTaskException
    {
        public static void Go()
        {
            // 注册未观察到异常的事件处理
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                // 处理任务异常
                Console.WriteLine($"Unobserved exception: {e.Exception}");
                // 设置为 true 表示我们已经处理了异常，防止应用崩溃
                // e.SetObserved();
            };

            // 创建一个没有被观察的 Task，抛出异常
            Task parent = Task.Factory.StartNew(TestThrow);

            parent = null;
            Console.ReadLine();
            // 强制进行垃圾回收，未观察的异常会被触发
            GC.Collect();
            // GC.WaitForPendingFinalizers();

            Console.ReadLine();
            Console.WriteLine("Program completed.");
        }

        static void TestThrow()
        {
            throw new InvalidOperationException();
        }
    }

    class TestFinally
    {
        public static void Go()
        {
        }
    }
}