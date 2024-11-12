using System;
using System.IO;

namespace Test
{
    public class TestCast
    {
        public static void Run()
        {
            // 创建要写入临时文件的字节
            Byte[] bytesToWrite = new Byte[] { 1, 2, 3, 4, 5 };

            // 创建临时文件
            using (FileStream fs = new FileStream("Temp.dat", FileMode.Create))
            {
                // 将字节写入临时文件
                fs.Write(bytesToWrite, 0, bytesToWrite.Length);
            }

            // 删除临时文件
            File.Delete("Temp.dat"); // 总能正常工作
        }
    }
}