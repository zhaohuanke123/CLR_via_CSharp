using System;
using System.Reflection;

namespace DelegateRemove
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Action a = null;
            Action b = () => { Console.WriteLine("lambda"); };
            a += b;
            a += b;
            a -= b;
            a.Invoke();
        }

        static void GoRefrlection()
        {
            var a1 = (Action)typeof(Program).GetMethod("Test", BindingFlags.Static | BindingFlags.NonPublic)
                .CreateDelegate(typeof(Action));
            a1 -= Test;
            Console.WriteLine(a1 == null);
        }

        static void Test()
        {
            Console.WriteLine("Test");
        }
    }
}