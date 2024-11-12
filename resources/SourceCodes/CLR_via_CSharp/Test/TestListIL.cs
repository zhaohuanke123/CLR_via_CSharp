using System;
using System.Collections.Generic;

namespace Test
{
    public class TestListIL
    {
        public static void Run()
        {
            List<int> list = new List<int>()
            {
                1,
                2,
                3,
                4,
                5
            };
            foreach (int num in list)
            {
                Console.WriteLine(num);
            }
        }
    }
}