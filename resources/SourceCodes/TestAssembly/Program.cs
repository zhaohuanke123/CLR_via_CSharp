extern alias Asm1;
extern alias Asm2;

namespace TestAssembly
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Asm1::Assembly1.SomeType st1 = new Asm1::Assembly1.SomeType();
            Asm2::Assembly1.SomeType st2 = new Asm2::Assembly1.SomeType();
        }
    }
}
