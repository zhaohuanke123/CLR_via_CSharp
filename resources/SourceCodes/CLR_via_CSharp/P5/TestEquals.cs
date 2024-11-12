namespace P5
{
    struct MyStruct
    {
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
    
    internal class TestEquals
    {
        public static void Main(string[] args)
        {
            TestDymaic.Run();
        }
    }
}