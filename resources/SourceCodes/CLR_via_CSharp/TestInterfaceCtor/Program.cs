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


    internal class Program : IF
    {
        public static void Main(string[] args)
        {
        }

        public void Dispose()
        {
        }

        public object Clone()
        {
            return null;
        }

        public IEnumerator GetEnumerator()
        {
            return null;
        }

        public void CopyTo(Array array, int index)
        {
        }

        public int Count { get; }
        public object SyncRoot { get; }
        public bool IsSynchronized { get; }
        public void Test()
        {
            
        }

        public void Test1()
        {
        }
    }
}