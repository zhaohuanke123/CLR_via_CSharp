using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace FileReadAsync
{
    internal class Program
    {
        private static object o;

        public static async Task Main(string[] args)
        {
            // 写一个异步文件读取
            string filePath = @"C:\Users\zhaohuanke\Desktop\Test.txt";

            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read, 1024,
                true);

            byte[] bytes = new byte[fs.Length];
            var task = fs.ReadAsync(bytes, 0, bytes.Length);

            var res = await task;
            var res1 = await task;
            var res2 = await task;
            var res3 = await task;

            for (int i = 0; i < 5; i++)
            {
                var res4 = await task;
            }

            for (int i = 0; i < 5; i++)
            {
                var res5 = await task;
            }

            await Task.CompletedTask;
        }
    }
}