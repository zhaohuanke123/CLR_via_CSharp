using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

class Program
{
    static void Main()
    {
        GCHandle gcHandle = GCHandle.Alloc(new object(), GCHandleType.Weak);
        // Test();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

        Console.WriteLine(gcHandle.Target == null);

        Console.ReadLine();
    }

    static void Test()
    {
        var c = new Class();
        c = null;
    }
}

class Class
{
    public Class()
    {
    }

    ~Class()
    {
        Console.WriteLine("Destructor");
    }
}