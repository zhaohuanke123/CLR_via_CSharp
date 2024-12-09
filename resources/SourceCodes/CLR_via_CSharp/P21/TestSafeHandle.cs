using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace P21
{
    internal class TestSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public TestSafeHandle(bool ownsHandle) : base(ownsHandle)
        {
            {
            }
        }

        protected override bool ReleaseHandle()
        {
            return true;
        }
    }
}