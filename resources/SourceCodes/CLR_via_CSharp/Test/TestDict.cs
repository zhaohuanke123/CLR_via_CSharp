using System;
using System.Collections.Generic;
using System.Security;

namespace Test
{
    public class TestDict
    {
        public static void RUn()
        {
            Dictionary<int, int> dict = new Dictionary<int, int>()
            {
                { 1, 1 },
                { 2, 2 },
                { 3, 3 },
                { 4, 4 },
                { 5, 5 },
                { 6, 6 },
                { 7, 7 },
                { 8, 8 },
                { 9, 9 },
                { 10, 10 },
            };
             
            foreach (var item in dict)
            {
                System.Console.WriteLine(item.Key + " " + item.Value);
            }
        }
    }
}