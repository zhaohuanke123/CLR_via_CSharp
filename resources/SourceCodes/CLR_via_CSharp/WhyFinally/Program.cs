using System;
using System.Runtime.Serialization;

namespace WhyFinally
{
    [Serializable]
    public class MyException : Exception
    {
        private string s;

        public MyException(string s)
        {
            this.s = s;
        }

        public MyException(string s, string message) : base(message)
        {
            this.s = s;
        }

        public MyException(string s, string message, Exception inner) : base(message, inner)
        {
            this.s = s;
        }

        protected MyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            s = info.GetString("s");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("s", s);
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            TestControl.Go();
        }
    }

    [Serializable]
    class TestControl
    {
        public static void Go()
        {
            // 创建新的 AppDomain
            AppDomain newDomain = AppDomain.CreateDomain("NewDomain");

            // 在新 AppDomain 中执行代码
            try
            {
                newDomain.DoCallBack(() =>
                {
                    try
                    {
                        // 在新 AppDomain 中抛出 MyException 异常
                        throw new MyException("MyException", "This is an un-serializable exception");
                    }
                    catch (Exception e)
                    {
                        // 捕捉并打印异常信息
                        Console.WriteLine($"Caught exception in new AppDomain: {e.GetType().Name} - {e.Message}");
                        throw;
                    }
                });
            }
            catch (Exception e)
            {
                // 捕捉传递到原始 AppDomain 中的异常
                Console.WriteLine($"Caught exception in original AppDomain: {e.GetType().Name} - {e.Message}");
            }
            finally
            {
                // 卸载新的 AppDomain
                AppDomain.Unload(newDomain);
            }
        }
    }

    class TestExceptionThrow
    {
        public static void Go()
        {
            try
            {
                Test();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("After finally block");
        }

        private static void Test()
        {
            try
            {
                TestInner();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
            finally
            {
                Console.WriteLine("In finally block");
            }
        }

        public static void TestInner()
        {
            throw new Exception();
        }
    }
}