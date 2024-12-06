using System;
using System.Collections.Generic;

namespace TestGeneric
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var t = typeof(Dictionary<,>);

            foreach (var arg in t.GetGenericArguments())
            {
                Console.WriteLine(arg);
            }

            // 创建封闭类型
            var closedType = t.MakeGenericType(typeof(string), typeof(int));
            // 创建实例
            var instance = Activator.CreateInstance(closedType);
            Console.WriteLine(instance.GetType());

            // 获取泛型的参数
            var genericArguments = closedType.GetGenericArguments();
            foreach (var arg in genericArguments)
            {
                Console.WriteLine(arg);
            }

            Test(1);
        }

        private static object o = 1;

        private static void Test<T>(T t) where T : IComparable
        {
            var compareTo = t.CompareTo(o);
            Console.WriteLine(compareTo);
        }
    }
}