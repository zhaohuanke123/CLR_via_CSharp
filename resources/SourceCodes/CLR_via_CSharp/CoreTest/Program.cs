using System.Reflection;

namespace CoreTest;

class Program
{
    struct MyStruct
    {
        public readonly int a;

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

    class TestClass
    {
        public readonly int ReadOnlyField = 1;
    }

    static void Main(string[] args)
    {
        // Create an instance of TestClass
        TestClass testInstance = new TestClass();

        // Print the initial value of the readonly field
        Console.WriteLine($"Before modification: ReadOnlyField = {testInstance.ReadOnlyField}");

        // Get the type of the class
        Type type = typeof(TestClass);

        // Use reflection to get the FieldInfo for the readonly field
        FieldInfo fieldInfo = type.GetField("ReadOnlyField",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (fieldInfo == null)
        {
            Console.WriteLine("Field not found.");
            return;
        }

        // Remove the readonly protection using FieldAttributes
        // Note: This requires unsafe code and works in .NET Framework or .NET Core.
        fieldInfo.SetValue(testInstance, 100);

        // Print the modified value of the readonly field
        Console.WriteLine($"After modification: ReadOnlyField = {testInstance.ReadOnlyField}");
    }
}