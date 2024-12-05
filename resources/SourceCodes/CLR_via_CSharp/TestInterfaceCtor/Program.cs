using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TestInterfaceCtor
{
    internal interface IF : ICloneable, ICollection, IDisposable
    {
        void Test();
        void Test1();
    }


    internal static class Program
    {
        public static void Main(string[] args)
        {
            
        }
    }
}