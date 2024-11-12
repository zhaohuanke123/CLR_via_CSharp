using System;

namespace P4
{
        struct MyStruct
        {
            public int a;
            public int b;

            public MyStruct Clone()
            {
                var clone = this.MemberwiseClone();
                return (MyStruct)clone;
            }

            public override string ToString()
            {
                return a + " : " + b;
            }
        }


        internal class TestMenberwiseClone
        {
            public static void Main(string[] args)
            {
                MyStruct ms = new MyStruct() { a = 1, b = 2 };
                var mm = ms.Clone();
                Console.WriteLine(mm);
            }
        }
}