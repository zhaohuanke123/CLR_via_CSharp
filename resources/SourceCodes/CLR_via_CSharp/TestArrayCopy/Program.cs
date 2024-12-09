using System;

namespace TestArrayCopy
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // 测试1: Array.Copy 在目标数组大小不足时抛出异常

            int[] source1 = { 1, 2, 3, 4, 5 };
            int[] target1 = new int[3] { 1, 2, 3 }; // 目标数组太小，无法容纳源数组的所有元素
            try
            {
                Array.Copy(source1, target1, source1.Length); // 应该抛出 ArgumentException
            }
            catch (Exception ex)
            {
                foreach (var i in target1)
                {
                    Console.WriteLine(i);
                }

                Console.WriteLine("Test 1 (Array.Copy) Exception: " + ex.GetType().Name + " - " + ex.Message);
            }

            // 测试2: Array.Copy 在目标数组类型不匹配时抛出异常

            object[] source2 = { "123", "321", 3 }; // 数组类型是 int[]
            string[] target2 = new string[3] { "hhh", "hhh", "hhh" }; // 目标数组类型是 string[]
            try
            {
                Array.Copy(source2, target2, source2.Length); // 应该抛出 ArrayTypeMismatchException
            }
            catch (Exception ex)
            {
                foreach (var i in target2)
                {
                    Console.WriteLine(i);
                }

                Console.WriteLine("Test 2 (Array.Copy) Exception: " + ex.GetType().Name + " - " + ex.Message);
            }

            // 测试3: Array.ConstrainedCopy 在目标数组大小不足时抛出异常
            try
            {
                int[] source = { 1, 2, 3, 4, 5 };
                int[] target = new int[3]; // 目标数组太小，无法容纳源数组的所有元素
                Array.ConstrainedCopy(source, 0, target, 0, source.Length); // 应该抛出 ArgumentException
            }
            catch (Exception ex)
            {
                Console.WriteLine("Test 3 (Array.ConstrainedCopy) Exception: " + ex.GetType().Name + " - " +
                                  ex.Message);
            }

            // 测试4: Array.ConstrainedCopy 在目标数组类型不匹配时抛出异常

            object[] source4 = { "123", "321", 3 }; // 数组类型是 int[]
            string[] target4 = new string[3]
            {
                "hhh",
                "hhh",
                "hhh"
            }; // 目标数组类型是 string[]
            try
            {
                Array.ConstrainedCopy(source4, 0, target4, 0, source4.Length); // 应该抛出 ArrayTypeMismatchException
            }
            catch (Exception ex)
            {
                foreach (var i in target4)
                {
                    Console.WriteLine(i);
                }

                Console.WriteLine("Test 4 (Array.ConstrainedCopy) Exception: " + ex.GetType().Name + " - " +
                                  ex.Message);
            }

            // 测试5: Array.ConstrainedCopy 在索引越界时抛出异常
            try
            {
                int[] source = { 1, 2, 3 };
                int[] target = new int[5];
                Array.ConstrainedCopy(source, 0, target, 6, source.Length); // 应该抛出 ArgumentOutOfRangeException
            }
            catch (Exception ex)
            {
                Console.WriteLine("Test 5 (Array.ConstrainedCopy) Exception: " + ex.GetType().Name + " - " +
                                  ex.Message);
            }
        }
    }
}