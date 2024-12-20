using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

class Class
{
    public void Test()
    {
        try
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();
            Console.WriteLine("Test");
        }
        finally
        {
            Console.WriteLine("Finally");
        }
    }

    ~Class()
    {
        Console.WriteLine("Destructor");
    }
}

internal class Program
{
    static void Main()
    {
        new Class().Test();
    }
}