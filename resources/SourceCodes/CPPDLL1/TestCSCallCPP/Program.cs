using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

class NativeMethods
{
    //[DllImport(@"D:\Study\CLR_via_CSharp\resources\SourceCodes\CPPDLL1\TestCSCallCPP\bin\Debug\net6.0\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
    [DllImport(@"Dll1", CallingConvention = CallingConvention.Cdecl)]
    public static extern int add(int a, int b);

    [DllImport(@"Dll1", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr getMessage1();

    [DllImport(@"Dll1", CallingConvention = CallingConvention.Cdecl)]
    public static extern SafeWaitHandle testPtr(SafeHandle p);
}

class Program
{
    static void Main(string[] args)
    {
        // 调用 C++ 的 Add 函数
        //int result = NativeMethods.add(3, 5);
        //Console.WriteLine($"3 + 5 = {result}");

        //// 调用 C++ 的 GetMessage 函数
        //IntPtr messagePtr = NativeMethods.getMessage1();
        //string message = Marshal.PtrToStringAnsi(messagePtr);
        //Console.WriteLine($"Message from C++: {message}");

        IntPtr p = new IntPtr(100);
        SafeHandle handle = new SafeWaitHandle(p, true);
        _ = NativeMethods.testPtr(handle);
        Console.WriteLine(handle.ToString());
    }
}
