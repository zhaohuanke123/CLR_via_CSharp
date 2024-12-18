namespace C2_1ManageredPtr
{
    struct MyStruct
    {
        public int x;
        public int y;

        public override bool Equals(object obj)
        {
            if (obj is MyStruct)
            {
                MyStruct other = (MyStruct)obj;
                return x == other.x && y == other.y;
            }

            return false;
        }

        class Program
        {
            static MyStruct myStruct;

            static void Main(string[] args)
            {
                ref MyStruct myStructRef = ref GetMyStruct();
                Console.WriteLine(myStructRef.Equals(myStructRef));
            }

            static ref MyStruct GetMyStruct()
            {
                return ref myStruct;
            }
        }
    }
}