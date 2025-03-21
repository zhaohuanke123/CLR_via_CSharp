﻿using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace TestCore6
{
    public class ArrayPerformance
    {
        private const int Rows = 10000;
        private const int Columns = 100;

        private int[,] twoDimensionalArray;
        private int[][] jaggedArray;

        [GlobalSetup]
        public void Setup()
        {
            // 初始化二维数组
            twoDimensionalArray = new int[Rows, Columns];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    twoDimensionalArray[i, j] = i * j;
                }
            }

            // 初始化交错数组
            jaggedArray = new int[Rows][];
            for (int i = 0; i < Rows; i++)
            {
                jaggedArray[i] = new int[Columns];
                for (int j = 0; j < Columns; j++)
                {
                    jaggedArray[i][j] = i * j;
                }
            }
        }

        [Benchmark]
        public int TwoDimensionalArrayRead()
        {
            int sum = 0;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    sum += twoDimensionalArray[i, j];
                }
            }

            return sum;
        }

        [Benchmark]
        public int JaggedArrayRead()
        {
            int sum = 0;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    sum += jaggedArray[i][j];
                }
            }

            return sum;
        }

        [Benchmark]
        public void TwoDimensionalArrayWrite()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    twoDimensionalArray[i, j] = i + j;
                }
            }
        }

        [Benchmark]
        public void JaggedArrayWrite()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    jaggedArray[i][j] = i + j;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            GCHandle gcHandle1;
            GCHandle gcHandle2;
            Test(out gcHandle1, out gcHandle2);
            GC.Collect(2, GCCollectionMode.Forced);
            Console.WriteLine(gcHandle1.Target == null);
            Console.WriteLine(gcHandle2.Target == null);
            // _ = BenchmarkRunner.Run<ArrayPerformance>();
        }

        static void Test(out GCHandle gcHandle1, out GCHandle gcHandle2)
        {
            gcHandle1 = GCHandle.Alloc(new object(), GCHandleType.Weak);
            gcHandle2 = GCHandle.Alloc(typeof(int), GCHandleType.Weak);
        }
    }
}