using System;
using System.Collections;
using System.Collections.Generic;

namespace P10
{
    internal class Program
    {
        public List<int> LS { get; set; } = new List<int>();

        public int Po_s { set; get; }

        public Program()
        {
            // 定义类型，构造实例，并初始化属性
            var o1 = new { Name = "Jeff", Year = 1964 };

// 在控制台上显示属性： Name=Jeff, Year=1964
            Console.WriteLine("Name={0}, Year={1}", o1.Name, o1.Year);

            var o2 = new { o1.Name, o1.Year };

            Console.WriteLine("Name={0}, Year={1}", o2.Name, o2.Year);

            Console.WriteLine(o1.Equals(o2));

            Tuple<int, string, int> tuple = Tuple.Create(1, "asd", 1);
        }

        public static void Main(string[] args)
        {
            Program p = new Program();
            // TestIEnumer test = new TestIEnumer()
            // {
            //     { "123", "123", "123" },
            // };
        }
    }

    class TestIEnumer : IEnumerable
    {
        List<string> m_list = new List<string>();

        public IEnumerator GetEnumerator()
        {
            return m_list.GetEnumerator();
        }

        public void Add(string s, string s2, string s3)
        {
            m_list.Add(s);
        }
    }
}