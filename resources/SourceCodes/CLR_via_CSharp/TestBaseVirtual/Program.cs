using System;

namespace TestBaseVirtual
{
    internal class Program
    {
        class Base
        {
            static Base()
            {
                Console.WriteLine("static" + nameof(Base));
            }
            public Base()
            {
                M();
            }

            public virtual void M()
            {
            }
        }

        class Derive : Base
        {
            // 这个没问题, 内联初始化会插入到最前面，后面才是调用父类构造器
            private Program p = new Program();

            public Derive()
            {
                // 在这里初始化，在调用父类构造器时候，会调用父类的M方法，此时p还没有初始化
                // p = new Program();
            }

            public override void M()
            {
                p.GetType();
            }
        }

        public static void Main(string[] args)
        {
            Derive d = new Derive();
            d.GetType();
        }
    }
}