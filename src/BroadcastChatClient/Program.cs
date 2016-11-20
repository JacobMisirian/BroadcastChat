using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BroadcastChatClient
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            TcpClient client = new TcpClient(args[0], Convert.ToInt32(args[1]));
            while (!client.Connected)
                ;
            StreamWriter writer = new StreamWriter(client.GetStream());
            StreamReader reader = new StreamReader(client.GetStream());

            new Thread(() => sendThread(writer)).Start();

            while (true)
            {
                string msg = reader.ReadLine();
                if (msg == "PING")
                {
                    writer.WriteLine("PONG");
                    writer.Flush();
                }
                else
                    Console.WriteLine(msg);
            }
        }

        private static void sendThread(StreamWriter writer)
        {
            while (true)
            {
                Thread.Sleep(500);
                Console.Write("> ");
                writer.WriteLine(Console.ReadLine());
                writer.Flush();
            }
        }
    }
}
