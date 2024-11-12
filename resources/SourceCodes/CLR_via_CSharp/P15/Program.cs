using System;

namespace P15
{
    [Flags]
    enum MyEnum : byte
    {
        C0 = 1,
        C1 = 2,
        C2 = 3,
        C3 = 4,
        C4 = 6
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            // Test1();
            // Test2();
            // Test3();
            // 测试内存占用
             
        }

        private static void Test3()
        {
            MyEnum em = (MyEnum)5;
            Console.WriteLine(em);
        }

        public static void Test1()
        {
            // Console.WriteLine(Enum.Format(typeof(MyEnum), 2, "G"));
            MyEnum[] MyEnums = (MyEnum[])Enum.GetValues(typeof(MyEnum));
            Console.WriteLine("Number of symbols defined: " + MyEnums.Length);
            Console.WriteLine("Value\tSymbol\n-----\t------");
            foreach (MyEnum c in MyEnums)
            {
                // 以十进制和常规格式显示每个符号
                Console.WriteLine("{0,5:D}\t{0:G}", c.ToString());
            }
        }

        public static void Test2()
        {
            var name = Enum.GetName(typeof(MyEnum), 1);
            Console.WriteLine(name);
        }
    }
}