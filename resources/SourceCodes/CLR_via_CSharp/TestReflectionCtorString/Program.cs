using System;

namespace TestReflectionCtorString
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var type = typeof(string);
            // 获取有参属性, 索引器 Chars
            var propertyInfo = type.GetProperty("Chars");
            Console.WriteLine(propertyInfo == null);

            string s = "123";
            var value = propertyInfo.GetValue(s, new object[] { 1 });
            Console.WriteLine(value);

            propertyInfo.SetValue(s, 2, new object[] { 1 });
        }
    }
}