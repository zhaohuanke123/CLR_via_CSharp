namespace TestStructCtroNonPara
{
    struct MyStruct
    {
        private int a;
        private int b;
        private int c;
        public MyStruct()
        {
            Console.WriteLine("MyStruct ctor");
            a = 1;
            b = 2;
            c = 3;
        }
    }

    internal class Program
    {
        public static void Main()
        {
            MyStruct[] arr = new MyStruct[2];
            var a = new MyStruct();
        }
    }
}