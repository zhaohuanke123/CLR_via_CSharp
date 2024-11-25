namespace CoreTest;

class Program
{
    struct MyStruct
    {
        public int a;
        //public int b;
        //public int c;
        //public int d;
        //public int e;
        //public int f;
        //public int g;
        //public int h;
        //public int i;


        public MyStruct(int a)
        {
            this.a = a;
            //this.b = a;
            //this.c = a;
            //this.d = a;
            //this.e = a;
            //this.f = a;
            //this.g = a;
            //this.h = a;
            //this.i = a;
        }
    }

    static void Main(string[] args)
    {
        MyStruct myStruct = new MyStruct();

        Console.WriteLine(myStruct);

        MyStruct myStruct1 = new MyStruct(1);
        Console.WriteLine(myStruct1);

        MyStruct ms = new MyStruct();

        Console.WriteLine(ms);
        MyStruct m1 = new MyStruct();

        Console.WriteLine(m1);
    }
}