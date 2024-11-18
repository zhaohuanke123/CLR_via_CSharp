using System;
using System.Collections.Generic;

namespace TestStruct
{
    internal class Program
    {
        interface IIncrementable
        {
           void Increment(); 
        }
        struct MyStruct : IIncrementable, IEquatable<MyStruct>
        {
            public int a;

            public void Increment()
            {
                a++;
            }
            
            void IIncrementable.Increment()
            {
                Increment();
            }

            public MyStruct(int a)
            {
                this.a = a;
            }

            public override int GetHashCode()
            {
                return a;
            }

            public bool Equals(MyStruct other)
            {
                return a == other.a;
            }

            public override bool Equals(object obj)
            {
                return obj is MyStruct other && Equals(other);
            }
        }

        public static void Test01()
        {
            List<MyStruct> list = new List<MyStruct>();
            list.Add(new MyStruct(10));
            list[0].Increment();
            ((IIncrementable)list[0]).Increment();
            Console.WriteLine(list[0].a);
        }

        public static void Test02()
        {
            Dictionary<object,int> dict = new Dictionary<object, int>();
            object o = new MyStruct(10);
            dict.Add(o, 10);

            foreach (var key in dict.Keys)
            {
                ((IIncrementable)key).Increment();
            }
            foreach (var pair in dict)
            {
               Console.WriteLine(((MyStruct)pair.Key).a + " " + pair.Value);
            }
            dict.TryGetValue(o, out int value);
            Console.WriteLine(value);
        }

        static void Main()
        {
            // Test01();
            Test02();
        }
    }
}