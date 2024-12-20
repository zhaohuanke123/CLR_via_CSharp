using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using TestCSCallCPP;

internal class SafeMemHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private class NativeMethods
    {
        //[DllImport(@"D:\Study\CLR_via_CSharp\resources\SourceCodes\CPPDLL1\TestCSCallCPP\bin\Debug\net6.0\Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        [DllImport(@"Dll1", CallingConvention = CallingConvention.Cdecl)]
        public static extern int add(int a, int b);

        [DllImport(@"Dll1", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getMessage1();

        [DllImport(@"Dll1", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr newPtr();
        [DllImport(@"Dll1", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool releasePtr(IntPtr p);
    }

    public SafeMemHandle(bool ownsHandle) : base(ownsHandle)
    {
        this.handle = NativeMethods.newPtr();
        //SetHandle(NativeMethods.newPtr());
    }

    protected override bool ReleaseHandle()
    {
        return NativeMethods.releasePtr(this.handle);
    }
}


static class Program
{
    // Testing harness that injects faults.
    private static bool _printToConsole = false;
    private static bool _workerStarted = false;

    private static void Usage()
    {
        Console.WriteLine("Usage:");
        // Assumes that application is named HexViewer"
        Console.WriteLine("HexViewer <fileName> [-fault]");
        Console.WriteLine(" -fault Runs hex viewer repeatedly, injecting faults.");
    }

    private static void ViewInHex(Object fileName)
    {
        _workerStarted = true;
        byte[] bytes;
        using (MyFileReader reader = new MyFileReader((String)fileName))
        {
            bytes = reader.ReadContents(20);
        }  // Using block calls Dispose() for us here.

        if (_printToConsole)
        {
            // Print up to 20 bytes.
            int printNBytes = Math.Min(20, bytes.Length);
            Console.WriteLine("First {0} bytes of {1} in hex", printNBytes, fileName);
            for (int i = 0; i < printNBytes; i++)
                Console.Write("{0:x} ", bytes[i]);
            Console.WriteLine();
        }
    }

    static void Main(string[] args)
    {
        if (args.Length == 0 || args.Length > 2 ||
            args[0] == "-?" || args[0] == "/?")
        {
            Usage();
            return;
        }

        String fileName = args[0];
        bool injectFaultMode = args.Length > 1;
        if (!injectFaultMode)
        {
            _printToConsole = true;
            ViewInHex(fileName);
        }
        else
        {
            Console.WriteLine("Injecting faults - watch handle count in perfmon (press Ctrl-C when done)");
            int numIterations = 0;
            while (true)
            {
                _workerStarted = false;
                Thread t = new Thread(new ParameterizedThreadStart(ViewInHex));
                t.Start(fileName);
                Thread.Sleep(1);
                while (!_workerStarted)
                {
                    Thread.Sleep(0);
                }
                t.Abort();  // Normal applications should not do this.
                numIterations++;
                if (numIterations % 10 == 0)
                    GC.Collect();
                if (numIterations % 10000 == 0)
                    Console.WriteLine(numIterations);
            }
        }
    }
}
