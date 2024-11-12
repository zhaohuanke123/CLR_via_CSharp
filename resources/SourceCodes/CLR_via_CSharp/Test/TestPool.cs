using System;

namespace CharsAndStrings
{
    public class TestPool
    {
        public static void Go()
        {
            string str1 = "hello";
            string str2 = "hello";

            // 检查是否是同一个引用
            Console.WriteLine(Object.ReferenceEquals(str1, str2) ? "同一引用" : "不同引用");
        }
    }
}