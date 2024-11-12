using System.Net.Sockets;
using System.Text;

namespace CharsAndStrings
{
    public class UTF16NetworkStreamReader
    {
        public static string ReadUTF16String(NetworkStream networkStream)
        {
            byte[] buffer = new byte[1024];
            StringBuilder result = new StringBuilder();
            Decoder utf16Decoder = Encoding.Unicode.GetDecoder();

            while (true)
            {
                int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break; // End of stream

                // Decode buffer into characters, maintaining state across calls
                char[] chars = new char[utf16Decoder.GetCharCount(buffer, 0, bytesRead)];
                utf16Decoder.GetChars(buffer, 0, bytesRead, chars, 0);
                result.Append(chars);
            }

            return result.ToString();
        }
    }
}