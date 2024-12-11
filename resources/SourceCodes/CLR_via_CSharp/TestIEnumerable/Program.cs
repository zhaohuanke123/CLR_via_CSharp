using System;
using System.Collections;
using System.Collections.Generic;

namespace TestIEnumerable
{
    internal class Program : IEnumerable<Program>
    {
        public static void Main(string[] args)
        {
            Program p = new Program();
            foreach (Program program in p)
            {
                Console.WriteLine(program);
            }
        }

        public IEnumerator<Program> GetEnumerator()
        {
            int[] a = { 1, 2, 3, 4, 5, 6 };
            foreach (int i in a)
            {
                yield return new Program();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}