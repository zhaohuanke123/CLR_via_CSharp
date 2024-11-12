using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace P9
{
    public delegate void MyDelegate(string message = "Hello from delegate!");

    public class TestPara
    {
        public static void Run()
        {
            dynamic list = 1;
            foreach (var i in list)
            {
            }
        }
    }

    class Test : IEnumerable<Test>
    {
        int[] array = new int[10];

        public IEnumerator<Test> GetEnumerator()
        {
            return null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return null;
        }
    }
}