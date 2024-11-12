using System;

namespace P16
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Array a;

            // 创建一维的 0 基数组，不包含任何元素
            a = new String[0];
            Console.WriteLine(a.GetType()); // "System.String[]"

            // 创建一维 0 基数组，不包含任何元素
            a = Array.CreateInstance(typeof(String), new Int32[] { 0 }, new Int32[] { 0 });
            Console.WriteLine(a.GetType()); // "System.String[]"

            // 创建一维 1 基数组，其中不包含任何元素
            a = Array.CreateInstance(typeof(String), new Int32[] { 0 }, new Int32[] { 1 });
            Console.WriteLine(a.GetType()); // "System.String[*]"  <-- 这个显示很奇怪，不是吗？

            Console.WriteLine();

            // 创建二维 0 基数组，其中不包含任何元素
            a = new String[0, 0];
            Console.WriteLine(a.GetType()); // "System.String[,]"

            // 创建二维 0 基数组，其中不包含任何元素
            a = Array.CreateInstance(typeof(String), new Int32[] { 0, 0 }, new Int32[] { 0, 0 });
            Console.WriteLine(a.GetType()); // "System.String[,]"

            //
            a = Array.CreateInstance(typeof(String), new Int32[] { 0, 0 }, new Int32[] { 0, 0 });
            Console.WriteLine(a.GetType()); // "System.String[,]"
        }
    }
}