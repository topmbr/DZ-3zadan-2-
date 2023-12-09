using System;
using System.Net.Sockets;
using System.Text;  
namespace ConsoleApp101
{
    internal class Program
    {
        static TcpClient tcpClient;
        static NetworkStream clientStream;

        static void Main(string[] args)
        {
            Console.WriteLine("=== Client ===");
            Console.Write("Введіть IP-адресу сервера: ");
            string ipAddress = Console.ReadLine();
            Console.Write("Введіть порт сервера: ");
            int port = int.Parse(Console.ReadLine());

            tcpClient = new TcpClient(ipAddress, port);
            clientStream = tcpClient.GetStream();

            Thread receiveThread = new Thread(new ThreadStart(ReceiveMessages));
            receiveThread.Start();

            string message;
            do
            {
                Console.Write("Введіть повідомлення (або 'Bye' для відключення): ");
                message = Console.ReadLine();

                byte[] sendBytes = Encoding.UTF8.GetBytes(message);
                clientStream.Write(sendBytes, 0, sendBytes.Length);

            } while (!message.Equals("Bye", StringComparison.OrdinalIgnoreCase));

            tcpClient.Close();
        }
        private static void ReceiveMessages()
        {
            while (true)
            {
                byte[] message = new byte[4096];
                int bytesRead;

                try
                {
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    break;
                }

                if (bytesRead == 0)
                    break;

                string receivedMessage = Encoding.UTF8.GetString(message, 0, bytesRead);
                Console.WriteLine($"Сервер: {receivedMessage}");
            }
        }

    }
}