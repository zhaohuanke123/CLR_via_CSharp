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
    }
}