using System;
using System.Reflection;

namespace TestDelegate
{
    public delegate void MyDelegate(int i);

    internal sealed class AClass
    {
        private event MyDelegate me;


        public static string str = "Hello World";

        public static void Method(MyDelegate myDelegate, int i)
        {
            myDelegate(i);
        }

        public void CallbackWithoutNewingADelegateObject()
        {
            int arg = 10;
            Method(
                delegate(int i) { Console.WriteLine(i); },
                arg);
        }
    }

    public sealed class Program
    {
        public static void Main(string[] args)
        {
            // AClass aClass = new AClass();
            // aClass.CallbackWithoutNewingADelegateObject();
            Program obj = new Program();
            var type = obj.GetType();
            var methodInfo = type.GetMethod("Test", BindingFlags.Instance | BindingFlags.Public);
            // methodInfo.Invoke(obj, null);

            var delegateTest = methodInfo.CreateDelegate(typeof(Action), obj) as Action;
            if (delegateTest != null)
            {
                delegateTest.Invoke();
            }
        }

        public void Test()
        {
            Console.WriteLine(this.GetType().Name);
        }
    }
}