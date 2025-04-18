using System.Net.Sockets;

namespace TestReadOnlySpan
{
    internal sealed class Program
    {
        public static void Main(string[] args)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < 1_000_000; i++)
            {
                list.Add(i);
            }

            ReadList(list.ToArray().AsSpan(10, 100));
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Listen();
            socket.AcceptAsync();
            // socket.ConnectAsync();
            // socket.SendAsync();
        }

        public static void ReadList(ReadOnlySpan<int> data)
        {
            int res = 0;
            foreach (int t in data)
            {
                res += t;
            }

            Console.WriteLine(res);
        }
    }
}