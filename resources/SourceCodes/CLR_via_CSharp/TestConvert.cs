using System;
using System.Text;

namespace CharsAndStrings
{
    public class TestConvert
    {
        public static void Go()
        {
            String s = "你好你好世界.";

            Encoding encodingUTF8 = Encoding.UTF8;


            Byte[] encodedBytes = encodingUTF8.GetBytes(s);
            var bytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, encodedBytes);

            Console.WriteLine("Encoded bytes: " +
                              BitConverter.ToString(encodedBytes));

            Console.WriteLine(BitConverter.ToString(bytes));

            Encoding encodingASCII = Encoding.Unicode;

            String decodedString = encodingUTF8.GetString(encodedBytes);

            Console.WriteLine("Decoded string: " + decodedString);

            Console.WriteLine(encodingASCII.GetString(bytes));
        }

        public static void TestConvertBigSmall()
        {
            // UTF-16 Little Endian (LE) to Big Endian (BE) conversion
            string originalString = "你好世界";
            Encoding utf16LE = Encoding.Unicode; // Little Endian
            Encoding utf16BE = Encoding.BigEndianUnicode; // Big Endian

            // Convert the string to UTF-16 LE bytes
            byte[] utf16LEBytes = utf16LE.GetBytes(originalString);

            // Convert UTF-16 LE bytes to UTF-16 BE bytes
            byte[] utf16BEBytes = Encoding.Convert(utf16LE, utf16BE, utf16LEBytes);

            // Convert back to string from UTF-16 BE bytes
            string convertedString = utf16BE.GetString(utf16BEBytes);

            Console.WriteLine("Original string: " + originalString);
            Console.WriteLine("UTF-16 LE bytes: " + BitConverter.ToString(utf16LEBytes));
            Console.WriteLine("UTF-16 BE bytes: " + BitConverter.ToString(utf16BEBytes));
            Console.WriteLine("Converted string: " + convertedString);
        }

        public static void TestEncoding()
        {
            Encoding utf8 = Encoding.UTF8;
            var encoder = utf8.GetEncoder();

// 示例字符串
            string originalString = "测试GetEncoder";

// 将字符串编码为字节数组
            byte[] encodedBytes = utf8.GetBytes(originalString);

// 创建一个字符数组来存储解码后的字符
            char[] decodedChars = new char[utf8.GetCharCount(encodedBytes, 0, encodedBytes.Length)];

// 使用 GetDecoder 解码字节数组
            var decoder = utf8.GetDecoder();
            decoder.GetChars(encodedBytes, 0, encodedBytes.Length, decodedChars, 0);

// 输出解码后的字符串
            Console.WriteLine("Decoded string: " + new string(decodedChars));
            // 使用 Encoder 编码字符串为字节数组
            char[] originalChars = originalString.ToCharArray();
            encodedBytes = new byte[encoder.GetByteCount(originalChars, 0, originalChars.Length, true)];
            encoder.GetBytes(originalChars, 0, originalChars.Length, encodedBytes, 0, true);
        }
    }
}