using System;
using System.Linq;

namespace TestString
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string s1 = "123123";
            string s2 = "1211231";

            Console.WriteLine(String.Compare(s1, s2, StringComparison.Ordinal));

            var copy = string.Copy(s1);
            Console.WriteLine(ReferenceEquals(copy, s1));

            Console.WriteLine("================");
            char[] chars = new char[100];
            s1.CopyTo(2, chars, 3, 2);
            for (int i = 0; i < chars.Length; i++)
            {
                char ch = chars[i];
                if (ch != 0)
                {
                    Console.WriteLine(i + " " + ch);
                }
            }

            Console.WriteLine("================");

            // 测试 join方法
            string[] strings = { "1", "2", "3" };
            Console.WriteLine(string.Join(",", strings).Split(','));

            // 测试字符串内插 和 + 的区别
            string name = "Tom";
            int age = 18;
            Console.WriteLine($"My name is {name}, I'm {age} years old.");

            Console.WriteLine("My name is " + name + ", I'm " + age + " years old.");
            Console.WriteLine("================");

            string s = String.Intern(new string('a', 10));
            string ss = new string('a', 10);
            string sss = string.Intern(new string('a', 10));

            Console.WriteLine(ReferenceEquals(s, ss));
            Console.WriteLine(ReferenceEquals(s, sss));

            Console.WriteLine(_ = s + "123" + ss + 123);
            Console.WriteLine($"{s}123{ss}123{123.ToString()}");
        }
    }
}