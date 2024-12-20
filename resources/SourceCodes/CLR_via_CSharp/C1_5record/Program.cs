namespace C1_5record
{
    readonly record struct MyRecord(int X, int Y);

    internal class Program
    {
        static void Main()
        {
            MyRecord mr = new MyRecord();
            Test(in mr);

            void Test(in MyRecord mr)
            {
                Console.WriteLine(mr.GetType().IsValueType);
                Console.WriteLine(mr.X);
                Console.WriteLine(mr.Y);
                Console.WriteLine(mr.ToString());
                Console.WriteLine(mr);
            }
        }

        void Test()
        {
            int i = 0;
            Test1();

            void Test1()
            {
                Console.WriteLine(i + 123);
            }
        }
    }
}