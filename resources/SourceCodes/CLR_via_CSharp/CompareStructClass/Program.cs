namespace CompareStructClass
{
    class Class1
    {
        public static Program p;

        static Class1()
        {
            p = new Program();
            Class2.p.GetType();
        }
    }

    class Class2
    {
        public static Program p;

        static Class2()
        {
            p = new Program();
            Class1.p.GetType();
        }
    }

    struct Struct1
    {
    }

    class Program
    {
        static void Main()
        {
            Class1 c1 = new Class1();
        }
    }
}