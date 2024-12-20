using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace TestCSCallCPP
{
    internal class MySafeFileHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        // Create a SafeHandle, informing the base class
        // that this SafeHandle instance "owns" the handle,
        // and therefore SafeHandle should call
        // our ReleaseHandle method when the SafeHandle
        // is no longer in use.
        private MySafeFileHandle()
            : base(true)
        {
        }
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        override protected bool ReleaseHandle()
        {
            // Here, we must obey all rules for constrained execution regions.
            return NativeMethods.CloseHandle(handle);
            // If ReleaseHandle failed, it can be reported via the
            // "releaseHandleFailed" managed debugging assistant (MDA).  This
            // MDA is disabled by default, but can be enabled in a debugger
            // or during testing to diagnose handle corruption problems.
            // We do not throw an exception because most code could not recover
            // from the problem.
        }
    }

    [SuppressUnmanagedCodeSecurity()]
    internal static class NativeMethods
    {
        // Win32 constants for accessing files.
        internal const int GenericRead = unchecked((int)0x80000000);

        // Allocate a file object in the kernel, then return a handle to it.
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern MySafeFileHandle CreateFile(String fileName,
           int dwDesiredAccess, FileShare dwShareMode,
           IntPtr securityAttrs_MustBeZero, FileMode dwCreationDisposition,
           int dwFlagsAndAttributes, IntPtr hTemplateFile_MustBeZero);

        // Use the file handle.
        [DllImport("kernel32", SetLastError = true)]
        internal static extern int ReadFile(MySafeFileHandle handle, byte[] bytes,
           int numBytesToRead, out int numBytesRead, IntPtr overlapped_MustBeZero);

        // Free the kernel's file object (close the file).
        [DllImport("kernel32", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal static extern bool CloseHandle(IntPtr handle);
    }

    // The MyFileReader class is a sample class that accesses an operating system
    // resource and implements IDisposable. This is useful to show the types of
    // transformation required to make your resource wrapping classes
    // more resilient. Note the Dispose and Finalize implementations.
    // Consider this a simulation of System.IO.FileStream.
    public class MyFileReader : IDisposable
    {
        // _handle is set to null to indicate disposal of this instance.
        private MySafeFileHandle _handle;

        public MyFileReader(String fileName)
        {
            // Security permission check.
            String fullPath = Path.GetFullPath(fileName);
            new FileIOPermission(FileIOPermissionAccess.Read, fullPath).Demand();

            // Open a file, and save its handle in _handle.
            // Note that the most optimized code turns into two processor
            // instructions: 1) a call, and 2) moving the return value into
            // the _handle field.  With SafeHandle, the CLR's platform invoke
            // marshaling layer will store the handle into the SafeHandle
            // object in an atomic fashion. There is still the problem
            // that the SafeHandle object may not be stored in _handle, but
            // the real operating system handle value has been safely stored
            // in a critical finalizable object, ensuring against leaking
            // the handle even if there is an asynchronous exception.

            MySafeFileHandle tmpHandle;
            tmpHandle = NativeMethods.CreateFile(fileName, NativeMethods.GenericRead,
                FileShare.Read, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            // An async exception here will cause us to run our finalizer with
            // a null _handle, but MySafeFileHandle's ReleaseHandle code will
            // be invoked to free the handle.

            // This call to Sleep, run from the fault injection code in Main,
            // will help trigger a race. But it will not cause a handle leak
            // because the handle is already stored in a SafeHandle instance.
            // Critical finalization then guarantees that freeing the handle,
            // even during an unexpected AppDomain unload.
            Thread.Sleep(500);
            _handle = tmpHandle;  // Makes _handle point to a critical finalizable object.

            // Determine if file is opened successfully.
            if (_handle.IsInvalid)
                throw new Win32Exception(Marshal.GetLastWin32Error(), fileName);
        }

        public void Dispose()  // Follow the Dispose pattern - public nonvirtual.
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        // No finalizer is needed. The finalizer on SafeHandle
        // will clean up the MySafeFileHandle instance,
        // if it hasn't already been disposed.
        // However, there may be a need for a subclass to
        // introduce a finalizer, so Dispose is properly implemented here.
        protected virtual void Dispose(bool disposing)
        {
            // Note there are three interesting states here:
            // 1) CreateFile failed, _handle contains an invalid handle
            // 2) We called Dispose already, _handle is closed.
            // 3) _handle is null, due to an async exception before
            //    calling CreateFile. Note that the finalizer runs
            //    if the constructor fails.
            if (_handle != null && !_handle.IsInvalid)
            {
                // Free the handle
                _handle.Dispose();
            }
            // SafeHandle records the fact that we've called Dispose.
        }

        public byte[] ReadContents(int length)
        {
            if (_handle.IsInvalid)  // Is the handle disposed?
                throw new ObjectDisposedException("FileReader is closed");

            // This sample code will not work for all files.
            byte[] bytes = new byte[length];
            int numRead = 0;
            int r = NativeMethods.ReadFile(_handle, bytes, length, out numRead, IntPtr.Zero);
            // Since we removed MyFileReader's finalizer, we no longer need to
            // call GC.KeepAlive here.  Platform invoke will keep the SafeHandle
            // instance alive for the duration of the call.
            if (r == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            if (numRead < length)
            {
                byte[] newBytes = new byte[numRead];
                Array.Copy(bytes, newBytes, numRead);
                bytes = newBytes;
            }
            return bytes;
        }
    }
}
